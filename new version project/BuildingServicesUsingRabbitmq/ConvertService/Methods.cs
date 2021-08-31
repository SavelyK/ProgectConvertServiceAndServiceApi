using ConvertService.Interfases;
using ConvertService.Models;
using DbInformation;
using DbInformation.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SautinSoft.Document;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ConvertService
{
    public class Methods : IMethods
    {
        public async Task SaveFileDbAsync(InformationDbContext context)
        {
            await Task.Run(() =>
            {
                do
                {
                 var factory = new ConnectionFactory() { HostName = "localhost" };
                     var connection = factory.CreateConnection();
                      var channel = connection.CreateModel();


                    channel.QueueDeclare(queue: "init1-queue",
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);
                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += async (sender, @event) =>
                    {
                        var body = @event.Body;
                        string message = Encoding.UTF32.GetString(body.ToArray());
                        FileInformation fileLoad = JsonSerializer.Deserialize<FileInformation>(message);
                        if (fileLoad.FileName != null)
                        {
                            StartService.countIndex++;

                            fileLoad.Index = StartService.countIndex;
                            await Task.Delay(200);
                            context.FileInformations.Add(fileLoad);
                            context.SaveChanges();
                            Console.WriteLine("файл сохранен в базу данных");
                        }

                    };
                    channel.BasicConsume(queue: "init1-queue",
                        autoAck: true,
                        consumer: consumer);
                } while (true);
            });
        }


        //public async Task SaveDocxModelAsync(InformationDbContext context, ConcurrentQueue<DocxItemModel> nameQueue, ConcurrentQueue<DocxItemModel> complitedQueue)
        //{
        //    await Task.Run(async () =>
        //    {
        //        while (true)
        //        {
        //            var factory = new ConnectionFactory() { HostName = "localhost" };
        //            var connection = factory.CreateConnection();
        //            var channel = connection.CreateModel();
        //            channel.QueueDeclare(queue: "init1-queue",
        //                durable: false,
        //                exclusive: false,
        //                autoDelete: false,
        //                arguments: null);
        //            var consumer = new EventingBasicConsumer(channel);
        //            consumer.Received += (sender, @event) =>
        //            {
        //                var body = @event.Body;
        //                string message = Encoding.UTF32.GetString(body.ToArray());
        //                FileInformation fileLoad = JsonSerializer.Deserialize<FileInformation>(message);
        //                if (fileLoad != null)
        //                {
        //                    StartService.countIndex++;

        //                    fileLoad.Index = StartService.countIndex;
        //                    context.FileInformations.Add(fileLoad);
        //                    context.SaveChanges();
        //                }
        //            };
        //            channel.BasicConsume(queue: "init1-queue",
        //                autoAck: true,
        //                consumer: consumer);
        //            await Task.Delay(100);
        //            var file =  context.FileInformations.FirstOrDefault(p => p.Status == "Upload");
        //            if (file != null)
        //            {
        //                file.Status = "InProgress";
        //                DocxItemModel docx = new DocxItemModel();
        //                docx.Id = file.Id;
        //                docx.Path = file.Path;
        //                docx.Status = file.Status;
        //                docx.FileName = file.FileName;
        //                nameQueue.Enqueue(docx);
        //                context.SaveChanges();
        //            }
        //            await Task.Delay(100);
        //            while (!complitedQueue.IsEmpty) 
        //            {
        //                DocxItemModel docx = new DocxItemModel();
        //                if (complitedQueue.TryDequeue(out docx))
        //                {
        //                    var fileComplited =  context.FileInformations.FirstOrDefault(p => p.Id == docx.Id);
        //                    fileComplited.Status = docx.Status;
        //                    context.SaveChanges();
        //                } 
        //            }

        //        } 
        //    });
        //}
        public async Task EnqueConvert(InformationDbContext context, ConcurrentQueue<DocxItemModel> nameQueue)
        {
            await Task.Run(() =>
            {
                while (true)
                {
                    var file = context.FileInformations.FirstOrDefault(p => p.Status == "Upload");
                    if (file != null)
                    {
                        file.Status = "InProgress";
                        DocxItemModel docx = new DocxItemModel();
                        docx.Id = file.Id;
                        docx.Path = file.Path;
                        docx.Status = file.Status;
                        docx.FileName = file.FileName;
                        docx.Index = file.Index;
                        nameQueue.Enqueue(docx);
                        new ManualResetEvent(false).WaitOne(100);
                        context.SaveChanges();
                       

                    }
                    else
                    {
                        new ManualResetEvent(false).WaitOne(1000);
                    }
                }
            });
        }

        public Task Convert(InformationDbContext context, ConcurrentQueue<DocxItemModel> nameQueue, int maxCount, ConcurrentQueue<DocxItemModel> complitedQueue)
        {

            while (true)
            {
                while (nameQueue.IsEmpty)
                {
                    new ManualResetEvent(false).WaitOne(200);
                }
                if (StartService.count < maxCount)
                {
                    DocxItemModel docxModel;

                    if (nameQueue.TryDequeue(out docxModel))
                    {

                        StartService.count++;

                        Task.Run(() =>
                      {
                          DocumentCore dc = DocumentCore.Load(docxModel.Path);
                          dc.Save(docxModel.Path.Replace(".docx", ".pdf"));
                          Console.WriteLine(docxModel.Path);
                          docxModel.FileName = docxModel.FileName.Replace(".docx", ".pdf");
                          docxModel.Status = "Complited";
                          
                          complitedQueue.Enqueue(docxModel);

                          
                          //ConnectionFactory factory = new ConnectionFactory() { HostName = "localhost" };
                          //var connection = factory.CreateConnection();
                          //var channel = connection.CreateModel();

                          //channel.QueueDeclare(queue: "convertservice",
                          //                      durable: false,
                          //                      exclusive: false,
                          //                      autoDelete: false,
                          //                      arguments: null);

                          //string message = JsonSerializer.Serialize(docxModel);
                          //var body = Encoding.UTF32.GetBytes(message);
                          //channel.BasicPublish(exchange: "",
                          //                     routingKey: "convertservice",
                          //                     basicProperties: null,
                          //                     body: body);

                          StartService.count--;
                      });
                    }
                }
            }
        }

        public async Task SaveComplitedAsync(ConcurrentQueue<DocxItemModel> complitedQueue, InformationDbContext context)
        {
            while (true)
            {
                while (complitedQueue.IsEmpty)
                {
                    await Task.Delay(100);
                }

                DocxItemModel docxModel;

                if (complitedQueue.TryDequeue(out docxModel))
                {
                    FileInformation file = context.FileInformations
                    .FirstOrDefault(dbModel =>
                    dbModel.Id == docxModel.Id);
                    file.Status = "Complited";
                    file.Path = docxModel.Path.Replace(".docx", ".pdf");
                    file.FileName = docxModel.FileName.Replace(".docx", ".pdf");
                    context.SaveChanges();
                }
            }
        }



            public void ServiceStart(InformationDbContext context, ConcurrentQueue<DocxItemModel> nameQueue)
            {
                var files = context.FileInformations.Where(p => p.Status == "InProgress")
                    .OrderBy(p => p.Index);

                foreach (var file in files)
                {
                    if (file != null)
                    {
                        DocxItemModel docx = new DocxItemModel();
                        docx.Id = file.Id;
                        docx.Path = file.Path;
                        docx.Status = file.Status;
                        docx.FileName = file.FileName;
                        nameQueue.Enqueue(docx);
                    }
                }

            }
        }
    }


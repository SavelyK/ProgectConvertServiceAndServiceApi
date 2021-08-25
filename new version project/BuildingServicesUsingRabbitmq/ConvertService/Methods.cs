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


        public async Task SaveDocxModelAsync(InformationDbContext context)
        {
            await Task.Run(async () =>
            {
                do
                {
                    await Task.Delay(1000);
                    var factory = new ConnectionFactory() { HostName = "localhost" };
                    var connection = factory.CreateConnection();
                    var channel = connection.CreateModel();
                    channel.QueueDeclare(queue: "init1-queue",
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);
                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (sender, @event) =>
                    {
                        var body = @event.Body;
                        string message = Encoding.UTF32.GetString(body.ToArray());
                        FileInformation file = JsonSerializer.Deserialize<FileInformation>(message);
                        if (file != null)
                        {
                            StartService.countIndex++;

                            file.Index = StartService.countIndex;
                            context.FileInformations.Add(file);
                            context.SaveChanges();
                        }

                    };
                    channel.BasicConsume(queue: "init1-queue",
                        autoAck: true,
                        consumer: consumer);
                } while (true);
            });
        }
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
                        nameQueue.Enqueue(docx);
                        context.SaveChanges();
                    }
                    else
                    {
                        new ManualResetEvent(false).WaitOne(1000);
                    }
                }
            });
        }

        public  Task Convert(InformationDbContext context, ConcurrentQueue<DocxItemModel> nameQueue, int maxCount)
        {

            while (true)
            {
                while (nameQueue.IsEmpty)
                {
                    new ManualResetEvent(false).WaitOne(200);
                }
                if (StartService.count < maxCount & !nameQueue.IsEmpty)
                {
                    DocxItemModel docxModel;

                    if (nameQueue.TryDequeue(out docxModel))
                    {

                        StartService.count++;
                       
                         Task.Run(() =>
                       {

                           Console.WriteLine(docxModel.Status);
                           string path = docxModel.Path;

                          
                               DocumentCore dc = DocumentCore.Load(path);
                               dc.Save(path.Replace(".docx", ".pdf"));
                           
                           FileInformation file = context.FileInformations
                           .FirstOrDefault(dbModel =>
                           dbModel.Id == docxModel.Id);
                           file.Status = "Complited";
                           file.Path = path.Replace(".docx", ".pdf");
                           file.FileName = docxModel.FileName.Replace(".docx", ".pdf");
                           context.SaveChanges();
                           ConnectionFactory factory = new ConnectionFactory() { HostName = "localhost" };
                           var connection = factory.CreateConnection();
                           var channel = connection.CreateModel();

                           channel.QueueDeclare(queue: "convertservice",
                                                durable: false,
                                                exclusive: false,
                                                autoDelete: false,
                                                arguments: null);

                           string message = JsonSerializer.Serialize(file);
                           var body = Encoding.UTF32.GetBytes(message);
                           channel.BasicPublish(exchange: "",
                                                routingKey: "convertservice",
                                                basicProperties: null,
                                                body: body);

                           StartService.count--;
                       });
                    }
                }
            }
        }



        public void ServiceStart(InformationDbContext context, ConcurrentQueue<DocxItemModel> nameQueue)
        {
            var files = context.FileInformations.Where(p => p.Status == "InProgress")
                .OrderBy(p => p.Index);
            if (files != null)
            {
                foreach (var file in files)
                {
                    DocxItemModel docx = new DocxItemModel();
                    docx.Id = file.Id;
                    docx.Path = file.Path;
                    docx.Status = file.Status;
                    docx.FileName = file.FileName;
                    nameQueue.Enqueue(docx);
                    //context.SaveChanges(); 
                }
            }
        }
    }
}

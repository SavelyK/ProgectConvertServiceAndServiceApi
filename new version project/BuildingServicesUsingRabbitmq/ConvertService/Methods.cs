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


        public async Task SaveDocxModelAsync(InformationDbContext context, ConcurrentQueue<DocxItemModel> nameQueue)
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
                        DocxItemModel docxItem = JsonSerializer.Deserialize<DocxItemModel>(message);
                        Console.WriteLine(docxItem.Path);
                        FileInformation file = new FileInformation();
                        file.Id = docxItem.Id;
                        file.Path = docxItem.Path;
                        file.FileName = docxItem.FileName;
                        file.Status = docxItem.Status;


                        context.FileInformations.Add(file);
                        nameQueue.Enqueue(docxItem);
                        context.SaveChanges();

                    };
                    channel.BasicConsume(queue: "init1-queue",
                        autoAck: true,
                        consumer: consumer);
                } while (true);
            });
        }

        public Task Convert(InformationDbContext context, ConcurrentQueue<DocxItemModel> nameQueue, int maxCount)
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
                           .First(dbModel =>
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
                           Console.WriteLine(StartService.count);
                       });
                    }

                }

            }
        }



    }
}

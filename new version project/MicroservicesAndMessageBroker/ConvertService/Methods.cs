using ConvertService.Interfases;
using ConvertService.Models;
using Microsoft.EntityFrameworkCore;
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
        public async Task  SaveDocxModelAsync(ApplicationContext context, ConcurrentQueue<DocxItemModel> nameQueue)
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
                    var message = Encoding.UTF32.GetString(body.ToArray());
                    DocxItemModel docxItem = JsonSerializer.Deserialize<DocxItemModel>(message);
                    Console.WriteLine(docxItem.Path);
                    
                    context.Database.EnsureCreated();
                    context.DocxItemModels.Add(docxItem);
                    nameQueue.Enqueue(docxItem);
                    context.SaveChanges();

                };
                channel.BasicConsume(queue: "init1-queue",
                    autoAck: true,
                    consumer: consumer);
            } while (true);
            });
        }

        public Task Convert(ApplicationContext context, ConcurrentQueue<DocxItemModel> nameQueue, int maxCount)
        {
            int count = 0;
            while (true) 
            { 

                while (nameQueue.IsEmpty)
                {
                    new ManualResetEvent(false).WaitOne(200);
                }
                DocxItemModel docxModel;
                if (nameQueue.TryDequeue(out docxModel) | count < maxCount)
                {
                 Task.Run(() =>
                {

                    Console.WriteLine(docxModel.Status);
                    count++;
                    string path = docxModel.Path;
                    byte[] fileBytes = File.ReadAllBytes(path);
                    using (MemoryStream docxStream = new MemoryStream(fileBytes))
                    {
                        DocumentCore dc = DocumentCore.Load(docxStream, new DocxLoadOptions());
                        dc.Save(path.Replace(".docx", ".pdf"));
                    }
                    DocxItemModel file = context.DocxItemModels
                    .First(dbModel =>
                    dbModel.Id == docxModel.Id);
                        file.Status = "Complited";

                    context.SaveChanges();
                    count--;
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
                    
                });
                 }
              
            }
        }


    
        }
    }

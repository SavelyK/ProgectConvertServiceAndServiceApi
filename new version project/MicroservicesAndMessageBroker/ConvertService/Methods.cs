
using AutoMapper;
using ConvertService.Interfases;
using ConvertService.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Repository_Application.Repositorys.Commands.SaveDocxFile;
using RepositoryDomain;
using RepositoryPersistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ConvertService
{
   public class Methods : IMethods
    {
        
        public async Task  SaveDocxModelAsync(ApplicationContext context)
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
                    context.SaveChanges();

                };
                channel.BasicConsume(queue: "init1-queue",
                    autoAck: true,
                    consumer: consumer);



            } while (true);
            });
        }
    }
}

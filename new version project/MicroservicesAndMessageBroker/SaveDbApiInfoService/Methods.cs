using DbInformation;
using DbInformation.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SaveDbApiInfoService.Interfases;
using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SaveDbApiInfoService
{
   public class Methods : IMethods
    {  
      
        

        public async Task SaveFileInformationAsync(InformationDbContext context)
        {
            await Task.Run(async () =>
            {
                do
                {
                    await Task.Delay(1000);
                    var factory = new ConnectionFactory() { HostName = "localhost" };
                    var connection = factory.CreateConnection();
                    var channel = connection.CreateModel();


                    channel.QueueDeclare(queue: "convertservice",
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);
                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (sender, @event) =>
                    {
                        var body = @event.Body;
                        var message = Encoding.UTF32.GetString(body.ToArray());
                        FileInformation fileInformation = JsonSerializer.Deserialize<FileInformation>(message);
                        Console.WriteLine(fileInformation.Path);

                        context.Database.EnsureCreated();
                        context.FileInformations.Add(fileInformation);
                        context.SaveChanges();

                    };
                    channel.BasicConsume(queue: "convertservice",
                        autoAck: true,
                        consumer: consumer);
                } while (true);
            });

           

        }
        }


    
        }
    
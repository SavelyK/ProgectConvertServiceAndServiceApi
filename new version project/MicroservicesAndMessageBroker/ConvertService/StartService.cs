using ConvertService.Interfases;
using ConvertService.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ConvertService
{
    public class StartService : IStartService
    {
      
            

public async Task Run()
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

                    };
                    channel.BasicConsume(queue: "init1-queue",
                        autoAck: true,
                        consumer: consumer);


                
            } while (true);
        }
       
    }
}

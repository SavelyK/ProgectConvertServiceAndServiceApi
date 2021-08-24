using ApiUpload.Models;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using RepositoryWebApi.Models;
using System;
using System.IO;
using System.Text;
using System.Text.Json;


namespace ApiUpload.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]



    public class UploadController : ControllerBase
    {
    ConnectionFactory factory = new ConnectionFactory() { HostName = "localhost" };
    [HttpPost]
        public  ActionResult<Guid> PostUploadFile([FromForm] FileUpload objectFile)
        {
            try
            {
                if (objectFile.files !=null)
                {
                        string path = "C:\\uploads\\";
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        string permittedExtensions = ".docx";
                        string name = objectFile.files.FileName;
                        var ext = Path.GetExtension(name).ToLowerInvariant();
                        if (string.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext))
                        {
                            return BadRequest("The extension is invalid ... discontinue processing the file");
                        }
                        else 

                            using (FileStream fileStream = System.IO.File.Create(path + objectFile.files.FileName))
                            {
                                objectFile.files.CopyTo(fileStream);
                                fileStream.Flush();
                               
                               

                                using (var connection = factory.CreateConnection())
                                using (var channel = connection.CreateModel())
                                {
                                    channel.QueueDeclare(queue: "init1-queue",
                                                            durable: false,
                                                            exclusive: false,
                                                            autoDelete: false,
                                                            arguments: null);
                                    channel.QueueDeclare(queue: "init2-queue",
                                                           durable: false,
                                                           exclusive: false,
                                                           autoDelete: false,
                                                           arguments: null);
                                    DocxItemModel docxItem = new DocxItemModel(Guid.NewGuid(), path + name, "Upload", name);
                                    string message = JsonSerializer.Serialize(docxItem);
                                    var body = Encoding.UTF32.GetBytes(message);
                                    channel.BasicPublish(exchange: "",
                                                            routingKey: "init1-queue",
                                                            basicProperties: null,
                                                            body: body);
                                    channel.BasicPublish(exchange: "",
                                                            routingKey: "init2-queue",
                                                            basicProperties: null,
                                                            body: body);
                                return Ok(docxItem.Id);
                                }
                            }
                 }
                
                
                else
                {
                    return BadRequest("the request must not be empty");
                }
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }
        

        }
}

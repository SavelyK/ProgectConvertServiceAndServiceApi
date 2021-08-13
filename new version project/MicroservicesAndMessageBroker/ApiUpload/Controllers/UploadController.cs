using Microsoft.AspNetCore.Mvc;
using RepositoryWebApi.Models;
using System;
using RabbitMQ.Client;
using System.IO;
using System.Text;
using AutoMapper;
using System.Threading.Tasks;
using Repository_Application.Repositorys.Commands.SaveDocxFile;
using ApiDownload.Models;
using System.Text.Json;

namespace RepositoryWebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : BaseController
    {
        private readonly IMapper _mapper;
        public UploadController(IMapper mapper)
        {
            _mapper = mapper;
        }
        ConnectionFactory factory = new ConnectionFactory() { HostName = "localhost" };


        [HttpPost]
        public async Task<ActionResult<Guid>> PostUploadFile([FromForm] FileUpload objectFile)
        {
       
        
            try
            {
                if (objectFile != null)
                {

                    if (objectFile.files.Length > 0)
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
                                SaveDocxRepositoryDto saveDocxRepositoryDto = new SaveDocxRepositoryDto();
                                saveDocxRepositoryDto.Path = path + name;
                                saveDocxRepositoryDto.FileName = name;
                                var command = _mapper.Map<SaveDocxRepositoryCommand>(saveDocxRepositoryDto);
                                command.UserId = UserId;
                                var repositoryId = await Mediator.Send(command);
                             
                                using (var connection = factory.CreateConnection())
                                using (var channel = connection.CreateModel())
                                {
                                    channel.QueueDeclare(queue: "init1-queue",
                                                            durable: false,
                                                            exclusive: false,
                                                            autoDelete: false,
                                                            arguments: null);
                                    DocxItemModel docxItem = new DocxItemModel(repositoryId, path + name, "Upload");
                                    string message = JsonSerializer.Serialize(docxItem);
                                    var body = Encoding.UTF32.GetBytes(message);
                                channel.BasicPublish(exchange: "",
                                                        routingKey: "init1-queue",
                                                        basicProperties: null,
                                                        body: body);
                                }
                                return Ok(repositoryId);
                            }
                    }
                    else
                    {
                        return BadRequest("the file must not be empty");
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

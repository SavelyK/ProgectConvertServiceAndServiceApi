using Microsoft.AspNetCore.Mvc;
using RepositoryWebApi.Models;
using System;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Http;
using AutoMapper;
using System.Threading.Tasks;
using Repository_Application.Repositorys.Commands.SaveDocxFile;

namespace RepositoryWebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : BaseController
    {
        public static IWebHostEnvironment _webHostEnvironment;
        private readonly IMapper _mapper;
        public UploadController(IWebHostEnvironment webHostEnvironment, IMapper mapper)
        {
            _webHostEnvironment = webHostEnvironment;
            _mapper = mapper;
        }

       

        [HttpPost]
        public async Task<ActionResult<Guid>> PostUploadFile([FromForm] FileUpload objectFile)
            {
            try
            {
                if (objectFile != null)
                {

                    if (objectFile.files.Length > 0)
                    {

                        string path = _webHostEnvironment.WebRootPath + "\\uploads\\";
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        string permittedExtensions = ".docx";
                        string name = objectFile.files.FileName;
                        Random rnd = new Random();
                        int value = rnd.Next(0, 4);
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
                                saveDocxRepositoryDto.FileLength = objectFile.files.Length;
                                saveDocxRepositoryDto.Path = path + name;
                                saveDocxRepositoryDto.FileName = name;
                                saveDocxRepositoryDto.Priority = value;
                                 var command = _mapper.Map<SaveDocxRepositoryCommand>(saveDocxRepositoryDto);
                                command.UserId = UserId;
                                var repositoryId = await Mediator.Send(command);
                                 return Ok("Upload " + repositoryId);
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

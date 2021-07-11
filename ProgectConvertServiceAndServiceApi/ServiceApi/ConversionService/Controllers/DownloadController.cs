using ConversionService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace ServiceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class DownloadController : ControllerBase
    {
        public static IWebHostEnvironment _webHostEnvironment;
        public DownloadController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        [HttpGet("{path}")]

        public string Get([FromForm] FileUpload objectFile)
        {
            try
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
                    int Id;
                    Random rnd = new Random();
                    int value = rnd.Next(0, 4);
                    var ext = Path.GetExtension(name).ToLowerInvariant();
                    if (string.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext))
                    {
                        return "The extension is invalid ... discontinue processing the file";
                    }
                    else

                        using (FileStream fileStream = System.IO.File.Create(path + objectFile.files.FileName))
                        {
                            using (var db = new MyDbContext())
                            {
                                var DbModels = db.Set<DbModel>();
                                DbModels.Add(new DbModel { Path = path + name, FileName = name, LoadTime = DateTime.Now, Status = 0, Priority = value, FileLength = objectFile.files.Length });
                                db.SaveChanges();
                            }
                            using (var db = new MyDbContext())
                            {
                                var file = db.DbModels.FirstOrDefault(p => p.Status == 0);
                                file.Status = 1;
                                Id = file.Id;
                                db.SaveChanges();
                            }
                            string TaskId = Id.ToString();
                            objectFile.files.CopyTo(fileStream);
                            fileStream.Flush();

                            return "Upload " + name + ";  Id задачи: " + TaskId;
                        }
                }
                else
                {
                    return "Not Uploaded";
                }
            }
            catch (Exception ex)
            {

                return ex.Message;
            }
        }
    }
}

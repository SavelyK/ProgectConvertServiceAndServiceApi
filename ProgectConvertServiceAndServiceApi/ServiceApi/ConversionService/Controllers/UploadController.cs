using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.AspNetCore.Hosting;
using ConversionService.Models;
using System.IO;
using System.Linq;



namespace ConversionService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        public static IWebHostEnvironment _webHostEnvironment;
        public UploadController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        [HttpPost]
        public string Post([FromForm] FileUpload objectFile)
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
                                DbModels.Add(new DbModel { Path = path + name, FileName = name, LoadTime = DateTime.Now, Status = DbModel.StatusProces.FileUpload, Priority = value, FileLength = objectFile.files.Length });
                                db.SaveChanges();
                            }
                            using (var db = new MyDbContext())
                            {
                                var file = db.DbModels.FirstOrDefault(p => p.Status == DbModel.StatusProces.FileUpload);
                                file.Status = DbModel.StatusProces.Wait;
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
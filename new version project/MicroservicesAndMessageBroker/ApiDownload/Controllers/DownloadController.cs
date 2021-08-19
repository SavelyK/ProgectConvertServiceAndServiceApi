using DbInformation;
using DbInformation.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiDownload.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]

    public class DownloadController : ControllerBase
    {
        private readonly InformationDbContext _context;
        public DownloadController(InformationDbContext context)
        {
            _context = context;
        }
    [HttpGet]
    [Route("{fileName}")]
        public ActionResult GetPdf(string fileName)
        {
            FileInformation file = _context.FileInformations
                   .FirstOrDefault(fileInformation =>
                   fileInformation.FileName == fileName);
            if (file == null)
            {
                return NotFound(); ;
            }
            else
            { 
            byte[] mas = System.IO.File.ReadAllBytes(file.Path);

            string fileType = "application/pdf";

            return File(mas, fileType, fileName);
            }

        }

    }
}

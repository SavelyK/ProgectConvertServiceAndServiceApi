using DbInformation;
using DbInformation.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiInformation.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class TaskStatusController : ControllerBase
    {
        private readonly InformationDbContext _context;
        public TaskStatusController(InformationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        [Route("{id}")]

        public ActionResult GetTaskStatus(Guid id)
        {
            FileInformation file = _context.FileInformations
                   .FirstOrDefault(fileInformation =>
                   fileInformation.Id == id);
            if(file != null)
            {
                return Ok($"{file.Status} {file.FileName}");
            }
            else
            { return NotFound(); }
        }
    }
}

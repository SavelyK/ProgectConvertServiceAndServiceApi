using ConversionService;
using ConversionService.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class TaskStatusController : ControllerBase
    {
        private readonly MyDbContext db;
        public TaskStatusController(MyDbContext db)
        {
            this.db = db;
        }

        /// <summary>
        /// Returns the status of the task. 
        /// If the status is complete - returns the file name
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Task status</returns>
        /// <response code="200">Success</response>
        /// <response code="404">if the request contains an incorrect id</response>

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<MyDbContext>> GetStatus(int id)
        {
            DbModel status = await db.DbModels.FirstOrDefaultAsync(x => x.Id == id);
            if (status.Status == DbModel.StatusProces.InProgress)
            {

                return Ok(new ObjectResult("status is being processed"));
            }
            if (status == null)
            {
                return NotFound();
            }
            else
                return Ok(new ObjectResult($"status completed. Path the file: {status.FileName}"));

        }

    }

}


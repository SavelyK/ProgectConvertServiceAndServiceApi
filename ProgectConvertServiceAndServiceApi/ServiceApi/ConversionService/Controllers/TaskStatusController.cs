using ConversionService;
using ConversionService.Models;
using Microsoft.AspNetCore.Hosting;
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
        /// <returns></returns>

            [HttpGet]
            [Route("{id}")]
             public async Task<ActionResult<MyDbContext>> Get(int id)
            {
                DbModel status = await db.DbModels.FirstOrDefaultAsync(x => x.Id == id);
            if (status.Status == DbModel.StatusProces.InProgress)
            {

                return new ObjectResult("status is being processed");
            }
            if(status == null)
            {
                return NotFound();
            }
            else
                return new ObjectResult($"status completed. Path the file: {status.FileName}");

            }

        }
        
    }


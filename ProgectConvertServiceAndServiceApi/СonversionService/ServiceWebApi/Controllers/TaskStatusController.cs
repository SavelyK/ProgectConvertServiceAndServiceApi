using LibraryModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServicePersistence;
using System;
using System.Threading.Tasks;

namespace ServiceWebApi.Controllers
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
        /// <response code="500">Internal Server Error</response>

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<MyDbContext>> GetStatus(int id)
        {
            try
            {
                DbModel status = await db.DbModels.FirstOrDefaultAsync(x => x.Id == id);
                if (status.Status != DbModel.StatusProces.Completed)
                {

                    return Ok(new ObjectResult("status is being processed"));
                }

                else if (status.Status == DbModel.StatusProces.Completed)
                {
                    return Ok(new ObjectResult($"status completed. Path the file: {status.FileName}"));
                }
                else
                {
                    return NotFound();
                }

            }
            catch (Exception)
            {
                return StatusCode(500);
            }

        }

    }
}

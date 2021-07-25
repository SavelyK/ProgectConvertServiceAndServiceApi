using Microsoft.AspNetCore.Mvc;
using RepositoryApplication.Repositorys.Queries.GetTaskStatus;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace RepositoryWebApi.Controllers
{
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    //[ApiVersionNeutral]
    [Produces("application/json")]
    [Route("api/{version:apiVersion}[controller]")]
    [ApiController]
    public class TaskStatusController : BaseController
    {

        [HttpGet]
        [Authorize]
        [Route("{id}")]
        public async Task<ActionResult<TaskStatusVm>> GetTaskStatus(Guid id)
        {
            var query = new GetTaskStatusQuery
            {
                UserId = UserId,
                Id = id
            };
            var vm = await Mediator.Send(query);
            if (vm.Status != "Completed")
            {
                return Ok(vm.Status);
            }
            else
            {
                return Ok(vm);
            }

        }

    }
}


using Microsoft.AspNetCore.Mvc;
using Repository_Application.Repositorys.Queries.GetTaskStatus;
using System;
using System.Threading.Tasks;

namespace RepositoryWebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class TaskStatusController : BaseController
    {

        [HttpGet]
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

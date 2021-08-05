using Microsoft.AspNetCore.Mvc;
using Repository_Application.Repositorys.Queries.GetPath;
using System;
using System.Threading.Tasks;

namespace RepositoryWebApi.Controllers
{
        [Produces("application/json")]
        [Route("api/[controller]")]
        [ApiController]
    public class DownloadController : BaseController
    {


        [HttpGet]
        [Route("{fileName}")]
        public async Task<ActionResult<PathVm>> GetPdf(string fileName)
        {
            var query = new GetPathQuery
            {
                UserId = UserId,
                FileName = fileName
            };
            var vm = await Mediator.Send(query);
            if (vm != null)
            {
                try
                {

                    byte[] mas = System.IO.File.ReadAllBytes(vm.Path);

                    string fileType = "application/pdf";

                    return  File(mas, fileType, fileName);

                }
                catch (Exception)
                {
                    return StatusCode(500);
                }
            }
            else
            {
                return NotFound();
            }
        }

    }
}

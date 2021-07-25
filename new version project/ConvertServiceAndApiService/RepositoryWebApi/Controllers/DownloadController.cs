using Microsoft.AspNetCore.Mvc;
using RepositoryApplication.Repositorys.Queries.GetPath;
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
    public class DownloadController : BaseController
    {


        [HttpGet]
        [Authorize]
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

                    return File(mas, fileType, fileName);

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

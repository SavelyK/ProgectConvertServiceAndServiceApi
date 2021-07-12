using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace ServiceApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class DownloadController : ControllerBase
    {

        private readonly IWebHostEnvironment _appEnvironment;
        public DownloadController(IWebHostEnvironment appEnvironment)
        {
            _appEnvironment = appEnvironment;
        }

        /// <summary>
        /// Downloads a file from the server
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns>Returns pdf file</returns>
        /// <response code="200">Success</response>
        /// <response code="404">if the request contains an incorrect file name</response>

        [HttpGet]
        [Route("{fileName}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetFile(string fileName)
        {
            fileName = $"wwwroot/uploads/{fileName}";
            string file_path = Path.Combine(_appEnvironment.ContentRootPath, fileName);

            string file_type = "application/pdf";

            return Ok(PhysicalFile(file_path, file_type));

        }
    }
}

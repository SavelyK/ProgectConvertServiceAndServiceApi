using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RepositoryDomain;
using RepositoryPersistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BalancingService.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class StartInstanceController : BaseController
    {
        private readonly RepositoryDbContext _context;
        public StartInstanceController(RepositoryDbContext context)
        {
            _context = context;
        }
        [HttpPost]
        [Route("{port}")]
        public async Task<ActionResult> PostSaveStatus(int port)
        {
            Repository repository;
            do
            {
                repository = await _context.Repositorys.FirstOrDefaultAsync(x => x.Port == port | x.Status == "InProgress");
                if (repository != null)
                {
                repository.Port = 0;
                repository.Status = "Wait";
                _context.SaveChanges();
                }
            }
            while (repository != null);
           
 
                return Ok("InstanceStart");


        }
    }
}

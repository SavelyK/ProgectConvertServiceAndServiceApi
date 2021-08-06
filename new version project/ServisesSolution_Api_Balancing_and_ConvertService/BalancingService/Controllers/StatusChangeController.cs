using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RepositoryPersistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BalancingService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusChangeController : BaseController
    {
        private readonly RepositoryDbContext _context;
        public StatusChangeController(RepositoryDbContext context)
        {
            _context = context;
        }
        [HttpPost]
        [Route("{id}")]
        public async Task<ActionResult> PostSaveStatus(Guid id)
        {
            var repository = await _context.Repositorys.FirstOrDefaultAsync(x => x.Id == id);
            if (repository == null)
            {
                return NotFound();
            }
            else
            {
                repository.Status = "Completed";
                _context.SaveChanges();
                return Ok();
            }

        }
    }
}

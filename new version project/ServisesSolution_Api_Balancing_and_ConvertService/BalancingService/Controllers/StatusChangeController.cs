using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RepositoryPersistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult> GetSaveStatus(Guid id)
        {
            CancellationToken cancellationToken = new CancellationToken();
            var repository = await _context.Repositorys.FirstOrDefaultAsync(x => x.Id == id);
            if (repository == null)
            {
                return NotFound();
            }
            else
            {
                repository.Status = "Completed";
                await _context.SaveChangesAsync(cancellationToken);
                return Ok();
            }

        }
    }
}

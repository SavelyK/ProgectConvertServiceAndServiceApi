﻿using BalancingService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RepositoryPersistence;
using System.Threading.Tasks;

namespace BalancingService.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]

    public class DownloadItemController : BaseController
    {
        private readonly RepositoryDbContext _context;
        public DownloadItemController(RepositoryDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<DocxItemModel>> PostDocxItem(int port)
        {
            var repository = await _context.Repositorys.FirstOrDefaultAsync(x => x.Status == "Wait");
            if(repository == null)
            {
                return NotFound();
            }
            else
            {
                repository.Port = port;
                repository.Status = "InProgres";
                DocxItemModel docxItem = new DocxItemModel(repository.Id, repository.Path,
                repository.LoadTime, repository.Priority, repository.FileLength );
                _context.SaveChanges();
                if(docxItem == null)
                {
                    return NotFound();
                }
                else
                return Ok(docxItem);
            }
            
        }


    }
}
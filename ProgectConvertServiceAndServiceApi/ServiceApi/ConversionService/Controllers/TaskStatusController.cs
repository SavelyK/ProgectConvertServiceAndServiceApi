using ConversionService;
using ConversionService.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskStatusController : ControllerBase
    {
        private readonly MyDbContext db;
        public TaskStatusController(MyDbContext db)
        {
            this.db = db;
        }
        string iscomplited;
        [HttpGet("id")]
        public string Get(int id)
        {
            int status;
            using (var db = new MyDbContext())
            {
                DbModel file;
                file = db.DbModels.FirstOrDefault(p => p.Id == id);
                 status = file.Status; 
            }
            if (status == 3)
            {
                iscomplited = "status completed";
                return iscomplited;
            }
            else
            {
                iscomplited ="status is being processed";
                return iscomplited;
            }
            
        }
        
    }
}

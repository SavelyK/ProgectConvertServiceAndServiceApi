using ConversionService;
using ConversionService.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        //string iscomplited;
        //[HttpGet]
        //public string Get(int id)
        //{
        //    int status;
        //    using (var db = new MyDbContext())
        //    {
        //        DbModel file;
        //        file = db.DbModels.FirstOrDefault(p => p.Id == id);
        //         status = file.Status; 
        //    }
        //    if (status == 3)
        //    {
        //        iscomplited = "status completed";
        //        return iscomplited;
        //    }
        //    else
        //    {
        //        iscomplited ="status is being processed";
        //        return iscomplited;
        //    }
            [HttpGet("{id}")]
             public async Task<ActionResult<MyDbContext>> Get(int id)
            {
                DbModel status = await db.DbModels.FirstOrDefaultAsync(x => x.Id == id);
            if (status.Status == 2)
            {

                return new ObjectResult("status is being processed");
            }
            else
                return new ObjectResult("status completed");


            }

        }
        
    }


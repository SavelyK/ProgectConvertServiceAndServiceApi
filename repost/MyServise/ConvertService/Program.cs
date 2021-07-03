using ConfigLibrary;
using LibraryModels;
using Microsoft.Extensions.Configuration;
using SautinSoft.Document;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ConvertService
{
    class Program
    {
        static void Main(string[] args)
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .Build();


            //Task t = new Task(() => Methods.MakePdfFileAsync(5,5,5,5,5));
            //t.Start();
            //Task v = new Task(() => Methods.MakeTaskAsync(2));
            //v.Start();
            //Task r = new Task(() => Methods.DbSortPrioriti1Async(5, 5, 5, 5, 5));
            //r.Start();
            //Console.ReadKey();
            ConcurrentQueue<Reserv>[] q =new ConcurrentQueue<Reserv>[5];
            Task t = new Task(() => Methods.CreativeQueueAsync(q));
            t.Start();
          
            


            using (var db = new MyDbContext())
            {
                var DbModels = db.Set<DbModel>();
                DbModel file = db.DbModels.FirstOrDefault();
                Print(DbModels);
                db.SaveChanges();
            }
        }
        static void Print(IEnumerable<DbModel> DbModels)
        {

            foreach (var DbModel in DbModels)
            {
                Console.WriteLine($"Id файла: {DbModel.Id}");
                Console.WriteLine($"Путь к файлу: {DbModel.Path}");
                Console.WriteLine($"Имя файла: {DbModel.FileName}");
                Console.WriteLine($"Время загрузки: {DbModel.LoadTime}");
                Console.WriteLine($"Индикация: {DbModel.Indicator}");
                Console.WriteLine($"приоритет: {DbModel.Priority}");
            }

        }
    }
}

using ConfigLibrary;
using LibraryModels;
using Microsoft.Extensions.Configuration;
using SautinSoft.Document;
using System;
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


            Task t = new Task(() => Methods.MakePdfFileAsync(5, 2));
            t.Start();
            //Task[] tasks = new Task[5]
            //{
            //    new Task(() => Methods.MakePdfFileAsync(1, 1)),
            //    new Task(() => Methods.MakePdfFileAsync(2,2)),
            //    new Task(() => Methods.MakePdfFileAsync(3,3)),
            //    new Task(() => Methods.MakePdfFileAsync(4,4)),
            //    new Task(() => Methods.MakePdfFileAsync(5,5))
            //};

            //foreach (var s in tasks)
            //    s.Start();
            Console.ReadKey();



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

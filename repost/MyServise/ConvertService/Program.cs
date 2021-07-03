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
            using (var db = new MyDbContext())
            {
                db.Database.EnsureCreated();
                db.SaveChanges();
            }

            Queue<Reserv>[] q =new Queue<Reserv>[5];
            Queue<Reserv> q1 = new Queue<Reserv>();
            Queue<Reserv> q2 = new Queue<Reserv>();
            Queue<Reserv> q3 = new Queue<Reserv>();
            Queue<Reserv> q4 = new Queue<Reserv>();
            Queue<Reserv> q5 = new Queue<Reserv>();

            q[0] = q1;
            q[1] = q2;
            q[2] = q3;
            q[3] = q4;
            q[4] = q5;


         

            Task t = new Task(() => Methods.EnqueueQueueAsync(q));
            t.Start();
            Task n = new Task(() => Methods.DequeueQueueAsync(q, 1));
            n.Start();
            Console.ReadKey();


            //test code block
            //using (var db = new MyDbContext())
            //{
            //    var DbModels = db.Set<DbModel>();
            //    DbModel file = db.DbModels.FirstOrDefault();
            //    Print(DbModels);
            //    db.SaveChanges();
            //}
        //}
        //static void Print(IEnumerable<DbModel> DbModels)
        //{

        //    foreach (var DbModel in DbModels)
        //    {
        //        Console.WriteLine($"Id файла: {DbModel.Id}");
        //        Console.WriteLine($"Путь к файлу: {DbModel.Path}");
        //        Console.WriteLine($"Имя файла: {DbModel.FileName}");
        //        Console.WriteLine($"Время загрузки: {DbModel.LoadTime}");
        //        Console.WriteLine($"Индикация: {DbModel.Indicator}");
        //        Console.WriteLine($"приоритет: {DbModel.Priority}");
        //    }

        }
    }
}

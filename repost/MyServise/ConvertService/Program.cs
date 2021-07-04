using ConfigLibrary;
using LibraryModels;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConvertService
{
    class Program
    {
        public static Queue<int> queueTaskId;
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
            queueTaskId = new Queue<int>();
            Queue<Reserv>[] q =new Queue<Reserv>[5];
            for (int i = 0; i < 5; i++)
            {
                q[i] = new Queue<Reserv>();
            }


            Task n = new Task(() =>
            {
                Methods.EnqueueQueueAsync(q);
                Methods.DequeueQueueAsync(q, 0);
                Methods.DequeueQueueAsync(q, 1);
                Methods.DequeueQueueAsync(q, 2);
                Methods.DequeueQueueAsync(q, 3);
                Methods.DequeueQueueAsync(q, 4);
                Methods.QueueLiquidatorAsync();
            });
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

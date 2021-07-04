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
            Queue<Reserv>[] priorityQueue =new Queue<Reserv>[5];
            for (int i = 0; i < 5; i++)
            {
                priorityQueue[i] = new Queue<Reserv>();
            }


            Task mainTask = new Task(() =>
            {
                Methods.EnqueueQueueAsync(priorityQueue);
                Methods.DequeueQueueAsync(priorityQueue, 0);
                Methods.DequeueQueueAsync(priorityQueue, 1);
                Methods.DequeueQueueAsync(priorityQueue, 2);
                Methods.DequeueQueueAsync(priorityQueue, 3);
                Methods.DequeueQueueAsync(priorityQueue, 4);
                Methods.QueueLiquidatorAsync();
            });
            mainTask.Start();
           

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

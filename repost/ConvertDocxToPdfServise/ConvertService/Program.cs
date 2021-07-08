using ConfigLibrary;
using LibraryModels;
using Microsoft.Extensions.Configuration;
using SautinSoft.Document;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ConvertService.Test;

namespace ConvertService
{
    class Program
    {

        private static readonly int limitedTasks = 5;
        public static int countTasks = 0;
        public static Queue<int> queueTaskId;
        static void Main(string[] args)
        {
            TaskScheduler scheduler = new LimitedConcurrencyTaskScheduler(limitedTasks);
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
               Task enqueueQueue = Methods.EnqueueQueueAsync(priorityQueue);
               //Task taskManager = Methods.TaskManager(priorityQueue);
               Task liquidatorQueue = Methods.QueueLiquidatorAsync();
            });
            mainTask.Start();
           
            Task taskConveer =  Task.Run(()  =>
            {
                while (true) 
                {
                   Reserv res =  Methods.SelectClient(priorityQueue);
                   Task taskConvert = new Task(() => Methods.Convert(res));
                   taskConvert.Start(scheduler);
                }
            });

            Console.ReadKey();


          

        }
    }
}

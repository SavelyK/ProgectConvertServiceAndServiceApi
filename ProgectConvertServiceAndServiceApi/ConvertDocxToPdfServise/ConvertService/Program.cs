using ConfigLibrary;
using LibraryModels;
using Microsoft.Extensions.Configuration;
using SautinSoft.Document;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace ConvertService
{
    class Program
    {

        internal static int[] priorityRatio = new int[5] { 1, 2, 3, 4, 5 }; //priority factor for selecting an item from queues with
                                                                                            //different priorities using a selection algorithm based on
                                                                                            //the waiting time and the given factor

        internal static int limitedTasks = 3;
        public static Queue<int> queueTaskId;
        static void Main(string[] args)
        {
            //limitedTasks = int.Parse(Console.ReadLine());
            TaskScheduler scheduler = new LimitedConcurrencyTaskScheduler(limitedTasks);
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .Build();
            using (var db = new MyDbContext())
            {
                db.Database.EnsureCreated();
                db.SaveChanges();
            }
            Methods.ServiceRestart();
            queueTaskId = new Queue<int>();
            Queue<Reserv>[] priorityQueue = new Queue<Reserv>[5];
            for (int i = 0; i < 5; i++)
            {
                priorityQueue[i] = new Queue<Reserv>();
            }




            Task mainTask = new Task(() =>
            {
                Task enqueueQueue = Methods.EnqueueQueueAsync(priorityQueue);
                Task liquidatorQueue = Methods.QueueLiquidatorAsync();
            });
            mainTask.Start();

            Task taskConveer = Task.Run(() =>
           {
               while (true)
               {
                   Reserv res = Methods.SelectClient(priorityQueue);
                   Task taskConvert = new Task(() => Methods.Convert(res));
                   taskConvert.Start(scheduler);
               }
           });

           Methods.CommandString();

            
        }
    }
}

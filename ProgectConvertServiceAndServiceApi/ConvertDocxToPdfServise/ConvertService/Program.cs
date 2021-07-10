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

        internal static double[] priorityRatio = new double[5] { 1.0, 1.9, 4.2, 7.3 , 9.0 }; //priority factor for selecting an item from queues with
                                                                                             //different priorities using a selection algorithm based on
                                                                                             //the waiting time and the given factor

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
            Methods.ServiceRestart();
            queueTaskId = new Queue<int>();
            Queue<Reserv>[] priorityQueue =new Queue<Reserv>[5];
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

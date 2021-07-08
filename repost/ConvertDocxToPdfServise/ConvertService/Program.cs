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
        public static int countTask = 0;
        public static int maxCountTask = 6;
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
               Task enqueueQueue = Methods.EnqueueQueueAsync(priorityQueue);
               Task taskManager = Methods.TaskManagerAsync(priorityQueue);
               Task liquidatorQueue = Methods.QueueLiquidatorAsync();
            });
            mainTask.Start();
           

            Console.ReadKey();


          

        }
    }
}

using ConvertDocxToPdfService.Interfases;
using ConvertDocxToPdfService.Models;
using Microsoft.Extensions.Configuration;
using RepositoryPersistence;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConvertDocxToPdfService
{
    public class StartConvertService : IStartConvertService
    {
        internal static int[] priorityRatio = new int[5] { 1, 2, 3, 4, 5 }; //priority factor for selecting an item from queues with
                                                                            //different priorities using a selection algorithm based on

        internal static int limitedTasks = 3;
        public static Queue<Guid> queueTaskId;

        public async Task Run()
        {
            Methods start = new Methods();                                                                  //the waiting time and the given factor
            TaskScheduler scheduler = new LimitedConcurrencyTaskScheduler(limitedTasks);
            queueTaskId = new Queue<Guid>();
            Queue<Reserv>[] priorityQueue = new Queue<Reserv>[5];
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .Build();
            await using (var db = new RepositoryDbContext())
            {
                db.Database.EnsureCreated();
                db.SaveChanges();
            }
            for (int i = 0; i < 5; i++)
            {
                priorityQueue[i] = new Queue<Reserv>();
            }
            start.ServiceRestart();




            Task mainTask = new Task(() =>
            {
                Task enqueueQueue = start.EnqueueQueueAsync(priorityQueue);
                Task liquidatorQueue = start.QueueLiquidatorAsync();
            });
            mainTask.Start();

            Task taskConveer = Task.Run(() =>
            {
                while (true)
                {
                    Reserv res = start.SelectClient(priorityQueue);
                    Task taskConvert = new Task(() => start.Convert(res));
                    taskConvert.Start(scheduler);
                }
            });
        }

    }



}

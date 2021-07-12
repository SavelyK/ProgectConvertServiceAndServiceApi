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

        internal static double[] priorityRatio = new double[5] { 1.0, 1.9, 4.2, 7.3, 9.0 }; //priority factor for selecting an item from queues with
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

            string input;
            List<string> words = new List<string>();
            string[] keywords = { "clear", "exit" };
            bool exit = false;
            while (!exit)
            {

                words.Clear();
                do
                {
                    input = Console.ReadLine();
                } while (input == "");
                string[] splitStr = input.Split(" ");
                foreach (string str in splitStr)
                {
                    if (str != "")
                    {
                        words.Add(str);
                    };
                }

                bool found = false;
                foreach (string key in keywords)
                {
                    if (words[0] == key)
                    {
                        found = true;
                    }
                }
                if (found)
                {
                    switch (words[0])
                    {

                        case "clear":
                            try
                            {
                                if (words.Count != 1) throw new Exception();
                                Console.Clear();
                            }
                            catch
                            {
                                Console.WriteLine("Invalid syntax");
                            }
                            break;
                        case "exit":
                            try
                            {
                                if (words.Count != 1) throw new Exception();
                                exit = true;
                            }
                            catch
                            {
                                if (words.Count != 1) throw new Exception();
                            }
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Keyword " + words[0] + " didn't found!");
                }
            }
        }
    }
}

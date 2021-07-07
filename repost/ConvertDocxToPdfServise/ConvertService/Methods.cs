using ConfigLibrary;
using LibraryModels;
using SautinSoft.Document;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ConvertService
{
    static class Methods
    {
        static object locker = new object();
        static object locker2 = new object();


       
        public async static Task QueueLiquidatorAsync() //a method that records completed tasks in the database
        {
            await Task.Run(() =>
            {
                using (var db = new MyDbContext())
                {
                    while (true)
                    {
                        if (Program.queueTaskId.Count() != 0)
                        {
                            int taskId = Program.queueTaskId.Dequeue();
                            var file = db.DbModels.FirstOrDefault(t => t.Id == taskId);
                            file.Status = 3;
                            db.SaveChanges();
                        }
                    }
                }
            });
        }


        public async static Task TaskManagerAsync(Queue<Reserv>[] nameArrayQueues) //this method selects and creates tasks
        {
             await Task.Run(() =>
            {
            // the algorithm for selecting an item from queues with different priorities for creating a task consists of three stages
            double[] queueWeight = new double[5]; // Stage 1: allocating space to store the "queue weight".
                                                  // "Queue weight" parameter by which an element is selected from an array of queues with different priorities
            Reserv res;
            while (true)
            {
                if (Program.countTask < Program.maxCountTask + 1)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        if (nameArrayQueues[i].Count() != 0)
                        {
                            //Stage 2: "Queue weight" calculation
                            //"Queue weight" = the waiting time for an item in the queue multiplied by the queue priority factor
                            queueWeight[i] = DateTime.Now.Subtract(nameArrayQueues[i].Peek().TimeRegistrInDb).TotalMilliseconds * (i + 1);
                        }
                        else
                        { queueWeight[i] = 0; }
                    }
                    if (queueWeight.Max() != 0)
                    {
                        res = nameArrayQueues[Array.IndexOf(queueWeight, queueWeight.Max())].Dequeue(); // Stage 3: Selecting an item to create a task
                        Task.Run( () =>       //block for creating a task for converting a file
                            {
                                char[] strPathFromArrayChar = res.FilePath.ToCharArray();
                                Program.queueTaskId.Enqueue(res.TaskId); 
                                string path = new string(strPathFromArrayChar);
                                Program.countTask++;
                                DocumentCore docPdf = DocumentCore.Load(path);
                                docPdf.Save(path.Replace(".docx", ".pdf"));
                                Program.countTask--;
                                Console.WriteLine(Program.countTask);
                            }); 
                        }
                    }
                }
            });    
        }
        public static void Convert(Reserv res)
        {
            string path = res.FilePath;
            Program.countTask++;
            DocumentCore docPdf = DocumentCore.Load(path);
            docPdf.Save(path.Replace(".docx", ".pdf"));
            Program.countTask--;
            Console.WriteLine(Program.countTask);
            Program.queueTaskId.Enqueue(res.TaskId);
        }
        public async static Task EnqueueQueueAsync(Queue<Reserv>[] nameArrayQueues) //method for creating a queue of data for processing by the task manager
        {
            Console.WriteLine("hello");
            await Task.Run(() =>
            {
                using (var db = new MyDbContext())
                {
                    while (true)
                    {
                        var file = db.DbModels.FirstOrDefault(t => t.Status == 1);
                        if (file != null)
                        {
                            file.Status = 2;
                            Reserv res = new Reserv(file.Id, file.Path, file.LoadTime);
                            db.SaveChanges();
                            nameArrayQueues[file.Priority].Enqueue(res);
                        }
                    }
                }
            });
        } 
        //public async static void DequeueQueueAsync(Queue<Reserv>[] nameArrayQueues, int numberPriorityQueue)
        //{
        //    await Task.Run(() =>
        //    {
        //        Console.WriteLine("hello");
        //        while (true)
        //        {
        //            Reserv res = new Reserv(0, null, DateTime.Now);
        //            lock (locker)
        //            {
        //                if (nameArrayQueues[numberPriorityQueue].Count() != 0)
        //                {
        //                    res = nameArrayQueues[numberPriorityQueue].Dequeue();
        //                }
        //            }
        //            if (res.FilePath != null)
        //            {
        //                string path = res.FilePath;
                       
        //                DocumentCore docPdf = DocumentCore.Load(path);
        //                docPdf.Save(path.Replace(".docx", ".pdf"));
        //                Program.queueTaskId.Enqueue(res.TaskId);
        //            }
        //        }
        //    });
        //} 
    }
}

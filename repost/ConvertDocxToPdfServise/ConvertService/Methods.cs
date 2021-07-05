using ConfigLibrary;
using LibraryModels;
using SautinSoft.Document;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ConvertService
{
    static class Methods
    {
        static object locker = new object();
        static object locker2 = new object();

        public async static void EnqueueQueueAsync(Queue<Reserv>[] nameArrayQueues)
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
        public async static void QueueLiquidatorAsync()
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

        public async static void DequeueQueueAsync(Queue<Reserv>[] nameArrayQueues, int numberPriorityQueue)
        {
            await Task.Run(() =>
            {
                Console.WriteLine("hello");
                while (true)
                {
                    Reserv res = new Reserv(0, null, DateTime.Now);
                    lock (locker)
                    {
                        if (nameArrayQueues[numberPriorityQueue].Count() != 0)
                        {
                            res = nameArrayQueues[numberPriorityQueue].Dequeue();
                        }
                    }
                    if (res.FilePath != null)
                    {
                        string path = res.FilePath;
                        DocumentCore docPdf = DocumentCore.Load(path);
                        docPdf.Save(path.Replace(".docx", ".pdf"));
                        Program.queueTaskId.Enqueue(res.TaskId);
                    }
                }
            });
        }
        public async static void TaskManagerAsync(Queue<Reserv>[] nameArrayQueues)
        {
            await Task.Run(() =>
            {
                double[] differenceInTime = new double[5];
                Reserv res;
                while (true)
                {
                    if (Program.countTask < Program.maxCountTask + 1)
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            if (nameArrayQueues[i].Count() != 0)
                            {
                                differenceInTime[i] = DateTime.Now.Subtract(nameArrayQueues[i].Peek().TimeRegistrInDb).TotalMilliseconds * (i + 1);
                            }
                            else
                            { differenceInTime[i] = 0; }
                        }
                        if (differenceInTime.Max() != 0)
                        {
                            res = nameArrayQueues[Array.IndexOf(differenceInTime, differenceInTime.Max())].Dequeue();

                            Task.Run(() =>
                            {
                                Program.countTask++;
                                ConvertDocxToPdf(res);
                                Program.countTask--;
                            });
                        }

                    }
                }
            });
        }
        public static void ConvertDocxToPdf(Reserv res)
        {
            string path = res.FilePath;
            DocumentCore docPdf = DocumentCore.Load(path);
            docPdf.Save(path.Replace(".docx", ".pdf"));
            Program.queueTaskId.Enqueue(res.TaskId);
        }
    }
}

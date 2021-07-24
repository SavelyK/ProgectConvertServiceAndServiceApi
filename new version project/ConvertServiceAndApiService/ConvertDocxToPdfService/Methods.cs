using ConvertDocxToPdfService.Models;
using RepositoryPersistence;
using SautinSoft.Document;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace ConvertDocxToPdfService
{
    public class Methods : IMethods
    {
        public async Task QueueLiquidatorAsync() //a method that records completed tasks in the database
        {
            await Task.Run(async () =>
            {
                using (var db = new RepositoryDbContext())
                {
                    while (true)
                    {
                        await Task.Delay(50);
                        if (StartConvertService.queueTaskId.Count() != 0)
                        {
                            Guid taskId = StartConvertService.queueTaskId.Dequeue();
                            var file = db.Repositorys.FirstOrDefault(t => t.Id == taskId);
                            if (file != null)
                            {
                                file.Status = "Completed";
                                file.Path = file.Path.Replace(".docx", ".pdf");
                                file.FileName = file.FileName.Replace(".docx", ".pdf");
                                db.SaveChanges();
                            }
                        }
                    }
                }
            });
        }


        public void ServiceRestart() //a method that saves tasks in the event of a service restart
        {
            using (var db = new RepositoryDbContext())
            {
                while (true)
                {
                    var file = db.Repositorys.FirstOrDefault(t => t.Status == "InProgress");
                    if (file != null)
                    {
                        file.Status = "Wait";
                        db.SaveChanges();
                    }
                    else break;
                }
            }
        }


        public Reserv SelectClient(Queue<Reserv>[] nameArrayQueues) //selection algorithm from queues with different priorities.
        {
            double[] queueWeight = new double[5];
            do
            {
                new ManualResetEvent(false).WaitOne(50); //alternative Task.Delay() for synchronous method
                for (int i = 0; i < 5; i++)
                {
                    queueWeight[i] = 0.0;
                }
                for (int i = 0; i < 5; i++)
                {
                    if (nameArrayQueues[i].Count() != 0)
                    {
                        queueWeight[i] = DateTime.Now.Subtract(nameArrayQueues[i].Peek().TimeRegistrInDb).TotalMilliseconds * StartConvertService.priorityRatio[i];
                    }
                    else
                    { queueWeight[i] = 0.0; }
                }
            }
            while (queueWeight.Max() == 0);

            return nameArrayQueues[Array.IndexOf(queueWeight, queueWeight.Max())].Dequeue();
        }



        public void Convert(Reserv res) //convert doc to pdf. Using package sautinsoft.socument.
        {
            Console.WriteLine($"задача на конвертацию файла номер: {Task.CurrentId} сработала в потоке: {Thread.CurrentThread.ManagedThreadId}.");
            var sw = new Stopwatch();
            sw.Start();
            string path = res.FilePath;
            byte[] fileBytes = File.ReadAllBytes(path);
            using (MemoryStream docxStream = new MemoryStream(fileBytes))
            {
                DocumentCore dc = DocumentCore.Load(docxStream, new DocxLoadOptions());
                dc.Save(path.Replace(".docx", ".pdf"));
            }
            sw.Stop();
            Console.WriteLine($"задача на конвертацию файла номер: {Task.CurrentId} закончила работу у потоке: {Thread.CurrentThread.ManagedThreadId} за время {sw.ElapsedMilliseconds} длина файла {res.FileLength}");
            StartConvertService.queueTaskId.Enqueue(res.TaskId);
        }

        public async Task EnqueueQueueAsync(Queue<Reserv>[] nameArrayQueues) //method for creating a queue of data for processing by the task manager
        {
            Console.WriteLine("start");
            await Task.Run(async () =>
            {
                using (var db = new RepositoryDbContext())
                {
                    while (true)
                    {
                        await Task.Delay(50);
                        var file = db.Repositorys.FirstOrDefault(t => t.Status == "Wait");
                        if (file != null)
                        {
                            file.Status = "InProgress";
                            Reserv res = new Reserv(file.Id, file.Path, file.LoadTime, file.FileLength);
                            db.SaveChanges();
                            nameArrayQueues[file.Priority].Enqueue(res);
                        }
                    }
                }
            });
        }
    }
}


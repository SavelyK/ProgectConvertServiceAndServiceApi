using ConfigLibrary;
using LibraryModels;
using SautinSoft.Document;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ConvertService
{
    static class Methods
    {

        public async static Task QueueLiquidatorAsync() //a method that records completed tasks in the database
        {
            await Task.Run(async() =>
            {
                await Task.Delay(100);
                using (var db = new MyDbContext())
                {
                    while (true)
                    {
                        if (Program.queueTaskId.Count() != 0)
                        {
                            int taskId = Program.queueTaskId.Dequeue();
                            var file = db.DbModels.FirstOrDefault(t => t.Id == taskId);
                            if (file != null)
                            { 
                            file.Status = 3;
                            db.SaveChanges();
                            }
                        }
                    }
                }
            });
        }

        public  static Reserv SelectClient(Queue<Reserv>[] nameArrayQueues) //selection algorithm from queues with different priorities.
        {
            double[] queueWeight = new double[5];
            do
            {
            for (int i = 0; i < 5; i++)
            {
                queueWeight[i] = 0.0;
            }
                for (int i = 0; i < 5; i++)
                {
                    if (nameArrayQueues[i].Count() != 0)
                    {
                        queueWeight[i] = DateTime.Now.Subtract(nameArrayQueues[i].Peek().TimeRegistrInDb).TotalMilliseconds * (Program.priorityRatio[i]);
                    }
                    else
                    { queueWeight[i] = 0.0; }
                }
            }
            while (queueWeight.Max() == 0);

             return  nameArrayQueues[Array.IndexOf(queueWeight, queueWeight.Max())].Dequeue(); 
        }

         

        public static void Convert(Reserv res) //convert doc to pdf. Using package sautinsoft.socument.
        {
           Console.WriteLine($"задача на конвертацию файла номер: {Task.CurrentId} сработала в потоке: {Thread.CurrentThread.ManagedThreadId}.");
           string path = res.FilePath;
            byte[] fileBytes = File.ReadAllBytes(path);
          using (MemoryStream docxStream = new MemoryStream(fileBytes)) 
            {
              DocumentCore  dc = DocumentCore.Load(docxStream, new DocxLoadOptions());
              dc.Save(path.Replace(".docx", ".pdf"));
            }
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
    }
}

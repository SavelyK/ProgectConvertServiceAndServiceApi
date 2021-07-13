using ConfigLibrary;
using LibraryModels;
using SautinSoft.Document;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace ConvertService
{
    static class Methods
    {

        internal async static Task QueueLiquidatorAsync() //a method that records completed tasks in the database
        {
            await Task.Run(async() =>
            {
                using (var db = new MyDbContext())
                {
                    while (true)
                    {
                            await Task.Delay(50);
                        if (Program.queueTaskId.Count() != 0)
                        {
                            int taskId = Program.queueTaskId.Dequeue();
                            var file = db.DbModels.FirstOrDefault(t => t.Id == taskId);
                            file.Status = DbModel.StatusProces.Completed;
                            file.Path = file.Path.Replace(".docx", ".pdf");
                            file.FileName = file.FileName.Replace(".docx", ".pdf");
                            db.SaveChanges();
                        }
                    }
                }
            });
        }
        

        internal static void ServiceRestart() //a method that saves tasks in the event of a service restart
        {
           using (var db = new MyDbContext())
           {
               while (true)
               { 
                   var file = db.DbModels.FirstOrDefault(t => t.Status == DbModel.StatusProces.InProgress);
                    if (file != null)
                    {
                        file.Status = DbModel.StatusProces.Wait;
                        db.SaveChanges();
                    }
                    else break;
               }
           }
        }


        internal  static Reserv SelectClient(Queue<Reserv>[] nameArrayQueues) //selection algorithm from queues with different priorities.
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
                        queueWeight[i] = DateTime.Now.Subtract(nameArrayQueues[i].Peek().TimeRegistrInDb).TotalMilliseconds * (Program.priorityRatio[i]);
                    }
                    else
                    { queueWeight[i] = 0.0; }
                }
            }
            while (queueWeight.Max() == 0);

             return  nameArrayQueues[Array.IndexOf(queueWeight, queueWeight.Max())].Dequeue(); 
        }

         

        internal static void Convert(Reserv res) //convert doc to pdf. Using package sautinsoft.socument.
        {
           Console.WriteLine($"задача на конвертацию файла номер: {Task.CurrentId} сработала в потоке: {Thread.CurrentThread.ManagedThreadId}.");
            var sw = new Stopwatch();
            sw.Start();
           string path = res.FilePath;
            byte[] fileBytes = File.ReadAllBytes(path);
          using (MemoryStream docxStream = new MemoryStream(fileBytes)) 
            {
              DocumentCore  dc = DocumentCore.Load(docxStream, new DocxLoadOptions());
              dc.Save(path.Replace(".docx", ".pdf"));
            }
            sw.Stop();
            Console.WriteLine($"задача на конвертацию файла номер: {Task.CurrentId} закончила работу у потоке: {Thread.CurrentThread.ManagedThreadId} за время {sw.ElapsedMilliseconds} длина файла {res.FileLength}");
           Program.queueTaskId.Enqueue(res.TaskId);
            
        }
     
        internal async static Task EnqueueQueueAsync(Queue<Reserv>[] nameArrayQueues) //method for creating a queue of data for processing by the task manager
        {
            Console.WriteLine("hello");
             await Task.Run(async() =>
            {
                using (var db = new MyDbContext())
                {
                    while (true)
                    {
                        await Task.Delay(50);
                        var file = db.DbModels.FirstOrDefault(t => t.Status == DbModel.StatusProces.Wait);
                        if (file != null)
                        {
                            file.Status = DbModel.StatusProces.InProgress;
                            Reserv res = new Reserv(file.Id, file.Path, file.LoadTime, file.FileLength);
                            db.SaveChanges();
                            nameArrayQueues[file.Priority].Enqueue(res);
                        }
                    }
                }
            });
        } 

       public static void CommandString()
        {
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

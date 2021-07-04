using ConfigLibrary;
using LibraryModels;
using SautinSoft.Document;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConvertService
{
    static class Methods
    {
        static object locker = new object();
        static object locker2 = new object();

        public async static void EnqueueQueueAsync(Queue<Reserv>[] q)
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
                            Reserv res = new Reserv(file.Id, file.Path);
                            db.SaveChanges();
                            q[file.Priority].Enqueue(res);
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
                            int x = Program.queueTaskId.Dequeue();
                            var file = db.DbModels.FirstOrDefault(t => t.Id == x);
                            file.Status = 3;
                            db.SaveChanges();
                        }
                    }
                }
            });
        }

        public async static void DequeueQueueAsync(Queue<Reserv>[] q, int i)
        {
            await Task.Run(() =>
            {
                Console.WriteLine("hello");
                while (true)
                {
                    Reserv res = new Reserv(0, null);
                    lock (locker)
                    {
                        if (q[i].Count() != 0)
                        {
                            res = q[i].Dequeue();
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
    }
}

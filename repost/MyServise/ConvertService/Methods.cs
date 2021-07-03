using ConfigLibrary;
using LibraryModels;
using SautinSoft.Document;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ConvertService
{
    static class Methods
    {
        static object locker = new object();
        public async static void EnqueueQueueAsync(Queue<Reserv> []q)
        {
            Console.WriteLine("hello");
            await Task.Run(() =>
            {
                using (var db = new MyDbContext())
                {
                    while (true)
                    {
                        var file = db.DbModels.FirstOrDefault(t => t.Indicator == 1);
                        if (file != null)
                        {
                            file.Indicator = 2;
                            Reserv t = new Reserv(file.Id, file.Path);
                            db.SaveChanges();
                            q[file.Priority - 1].Enqueue(t);
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
                    lock (locker) { 
                        if (q[i].Count() != 0)
                        {
                            Reserv res = q[i].Dequeue();
                            string path = res.FilePath;
                            DocumentCore docPdf = DocumentCore.Load(path);
                            docPdf.Save(path.Replace(".docx", ".pdf"));
                            
                        }
                    }
                }
            });
        }

        //public async static void MakeTaskAsync(int x)
        //{
        //    await using (var db = new MyDbContext())
        //    {
        //        while (true)
        //        {
        //            for (int i = 1; i < x + 1; i++)
        //            {
        //                var file = db.DbModels.FirstOrDefault(t => t.Priority == 1);
        //                if(file != null)
        //                { 
        //                if (file != null)
        //                file.TaskIndicator1 = i;
        //                file.Priority = 6;
        //                db.SaveChanges();
        //                }
        //            }
        //        }
        //    }
        //}



        //public async static void MakePdfFileAsync(int x,int y, int z, int t, int v)
        //{
        //  await using (var db = new MyDbContext())
        //    {
        //        while (true)
        //        {
        //            Task[] tasks1 = new Task[x];
        //            for (int i = 0; i < x; i++)
        //            tasks1[i] = new Task(() => 
        //            {
        //                var file = db.DbModels.FirstOrDefault(t => t.Priority == 6 | t.TaskIndicator1 == i + 1) ;
        //                if (file != null)
        //                {
        //                    string path = file.Path;
        //                    DocumentCore docPdf = DocumentCore.Load(path);
        //                    path = path.Replace(".docx", ".pdf");
        //                    docPdf.Save(path);
        //                    db.SaveChanges();
        //                }
        //                foreach (var s in tasks1)
        //                       s.Start();
        //            }
        //            );
        //            Task[] tasks2 = new Task[y];
        //            for (int i = 0; i < y; i++)
        //                tasks2[i] = new Task(() =>
        //                {
        //                    var file = db.DbModels.FirstOrDefault(t => t.Priority == 7 | t.TaskIndicator2 == i + 1);
        //                    if (file != null)
        //                    {
        //                        string path = file.Path;
        //                        DocumentCore docPdf = DocumentCore.Load(path);
        //                        path = path.Replace(".docx", ".pdf");
        //                        docPdf.Save(path);
        //                        db.SaveChanges();
        //                    }
        //                    foreach (var s in tasks2)
        //                        s.Start();
        //                }
        //                );
        //            Task[] tasks3 = new Task[z];
        //            for (int i = 0; i < z; i++)
        //                tasks3[i] = new Task(() =>
        //                {
        //                    var file = db.DbModels.FirstOrDefault(t => t.Priority == 8 | t.TaskIndicator3 == i + 1);
        //                    if (file != null)
        //                    {
        //                        string path = file.Path;
        //                        DocumentCore docPdf = DocumentCore.Load(path);
        //                        path = path.Replace(".docx", ".pdf");
        //                        docPdf.Save(path);
        //                        db.SaveChanges();
        //                    }
        //                    foreach (var s in tasks3)
        //                        s.Start();
        //                }
        //                );
        //            Task[] tasks4 = new Task[t];
        //            for (int i = 0; i < t; i++)
        //                tasks4[i] = new Task(() =>
        //                {
        //                    var file = db.DbModels.FirstOrDefault(t => t.Priority == 9 | t.TaskIndicator4 == i + 1);
        //                    if (file != null)
        //                    {
        //                        string path = file.Path;
        //                        DocumentCore docPdf = DocumentCore.Load(path);
        //                        path = path.Replace(".docx", ".pdf");
        //                        docPdf.Save(path);
        //                        db.SaveChanges();
        //                    }
        //                    foreach (var s in tasks1)
        //                        s.Start();
        //                }
        //                );
        //            Task[] tasks5 = new Task[v];
        //            for (int i = 0; i < v; i++)
        //                tasks5[i] = new Task(() =>
        //                {
        //                    var file = db.DbModels.FirstOrDefault(t => t.Priority == 10 | t.TaskIndicator5 == i + 1);
        //                    if (file != null)
        //                    {
        //                        string path = file.Path;
        //                        DocumentCore docPdf = DocumentCore.Load(path);
        //                        path = path.Replace(".docx", ".pdf");
        //                        docPdf.Save(path);
        //                        db.SaveChanges();
        //                    }
        //                    foreach (var s in tasks1)
        //                        s.Start();
        //                }
        //                );
        //        }
        //     }
        //}

        //public async static void DbSortPrioriti1Async (int x, int y, int z, int t, int v)
        //{
        //    await using (var db = new MyDbContext())
        //    {

        //            for (int i = 1; i < x + 1; i++)
        //            {
        //                var file = db.DbModels.FirstOrDefault(t => t.Priority == 1);
        //                if (file != null) 
        //                { 
        //                file.TaskIndicator1 = i;
        //                file.Priority = 6;
        //                db.SaveChanges();
        //                }
        //            }
        //            for (int i = 1; i < y + 1; i++)
        //            {
        //                var file = db.DbModels.FirstOrDefault(t => t.Priority == 2);
        //                if (file != null) 
        //                { 
        //                    file.TaskIndicator2 = i;
        //                    file.Priority = 7;
        //                    db.SaveChanges();
        //                }
        //            }
        //            for (int i = 1; i < z + 1; i++)
        //            {
        //                var file = db.DbModels.FirstOrDefault(t => t.Priority == 3);
        //                if (file != null)
        //                { 
        //                     file.TaskIndicator3 = i;
        //                     file.Priority = 8;
        //                     db.SaveChanges();
        //                }
        //            }
        //            for (int i = 1; i < t + 1; i++)
        //            {
        //                var file = db.DbModels.FirstOrDefault(t => t.Priority == 4);
        //                if (file != null) 
        //                { 
        //                    file.TaskIndicator4 = i;
        //                    file.Priority = 9;
        //                     db.SaveChanges();
        //                }
        //            }
        //            for (int i = 1; i < v + 1; i++)
        //            {
        //                var file = db.DbModels.FirstOrDefault(t => t.Priority == 5);
        //                if (file != null) 
        //                 { 
        //                     file.TaskIndicator5 = i;
        //                     file.Priority = 10;
        //                     db.SaveChanges();
        //                 }
        //            }

        //    }
        //}

    }
}

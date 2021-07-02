using ConfigLibrary;
using LibraryModels;
using SautinSoft.Document;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConvertService
{
    static class Methods
    {
        public async static void MakePdfFileAsync(int p, int x)
        {
          await using (var db = new MyDbContext())
            {
                while (true)
                {
                    for (int i = 1; i < x+1; i++)
                    {
                    var file = db.DbModels.FirstOrDefault(t => t.Priority == p);
                        if (file != null)
                            file.TaskIndicator = i;
                            db.SaveChanges();
                    }

                    Task[] tasks = new Task[x];
                    for (int i = 0; i < x; i++)
                    tasks[i] = new Task(() => 
                    { 
                        DbModel file = db.DbModels.FirstOrDefault(t => t.TaskIndicator == i+1);
                        Converter(file, db);
                    }
                    );
                    foreach (var t in tasks)
                        t.Start();
                }
          }
        }
        public static void Converter(DbModel file, MyDbContext db)
        {
            if (file != null)
            {
                string path = file.Path;
                DocumentCore docPdf = DocumentCore.Load(path);
                path = path.Replace(".docx", ".pdf");
                docPdf.Save(path);
                file.Priority = 6;
                db.SaveChanges();
            }
        }
            //public static void ConvertQueue()
            //{
            //    using (var db = new MyDbContext())
            //    {
            //        while (true)
            //        {
            //            var file = db.DocxFiles.FirstOrDefault(p => p.Indicator == 1);
            //            if (file != null)
            //            {
            //                QueueTask taskelement = tasks.Dequeue();
            //                file.Indicator = 2;
            //                db.QueueTasks.Remove(taskelement);
            //                db.SaveChanges();
            //                string id = file.Path;
            //                DocumentCore dc = DocumentCore.Load(id);
            //                id = id.Replace(".docx", ".pdf");
            //                dc.Save(id);

            //            }
            //            else
            //                Thread.Sleep(500);
            //        }

            //    }
            //}

            //public static void ConvertDocxToPdf()
            //{
            //    using (var db = new MyDbContext())
            //    {
            //        var file = db.DbModels.FirstOrDefault(p => p.Indicator == 0);
            //        if (file != null) 
            //        { 
            //            file.Indicator = 1;
            //            db.SaveChanges();
            //            string id = file.Path;
            //            DocumentCore dc = DocumentCore.Load(id);
            //            id = id.Replace(".docx", ".pdf");
            //            dc?.Save(id);
            //        }
            //    }
            //}
        }
}

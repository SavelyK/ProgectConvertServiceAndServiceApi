using ConfigLibrary;
using LibraryModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace ConvertService
{
    public static class TaskTimer
    {

        public static Timer timer;

        public static void SetTimer()
        {
            timer = new Timer(100);
            timer.Elapsed += OnTimedEventAsync;
            timer.AutoReset = true;
            timer.Enabled = true;
        }

        public static async void  OnTimedEventAsync(Object source, ElapsedEventArgs e)
        {
            await using (var db = new MyDbContext())
            {
                while (true)
                {
                    var file = db.DbModels.FirstOrDefault(t => t.Status == 1);
                    if (file != null)
                    {
                        file.Status = 2;
                        Reserv res = new Reserv(file.Id, file.Path);
                        db.SaveChanges();
                        nameArrayQueues[file.Priority].Enqueue(res);
                    }

                }
            }
        }
    }
}

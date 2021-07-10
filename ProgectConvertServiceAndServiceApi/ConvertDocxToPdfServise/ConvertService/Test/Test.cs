using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ConvertService.Test
{
    public static class Test
    {
        public static void ShowTheadPoolInfo(object _)
        {
            ThreadPool.GetAvailableThreads(out int threads, out int completionPorts);
            ThreadPool.GetMaxThreads(out int maxThreads, out int maxCompletionPorts);
            Console.WriteLine($"                    Worker Threads - [{threads}:{maxThreads}]");
            Console.WriteLine($"                    Completion Ports -[{completionPorts}:{maxCompletionPorts}]");

        }
        //test code block
        //using (var db = new MyDbContext())
        //{
        //    var DbModels = db.Set<DbModel>();
        //    DbModel file = db.DbModels.FirstOrDefault();
        //    Print(DbModels);
        //    db.SaveChanges();
        //}
        //}
        //static void Print(IEnumerable<DbModel> DbModels)
        //{

        //    foreach (var DbModel in DbModels)
        //    {
        //        Console.WriteLine($"Id файла: {DbModel.Id}");
        //        Console.WriteLine($"Путь к файлу: {DbModel.Path}");
        //        Console.WriteLine($"Имя файла: {DbModel.FileName}");
        //        Console.WriteLine($"Время загрузки: {DbModel.LoadTime}");
        //        Console.WriteLine($"Индикация: {DbModel.Indicator}");
        //        Console.WriteLine($"приоритет: {DbModel.Priority}");
        //    }
    }
}

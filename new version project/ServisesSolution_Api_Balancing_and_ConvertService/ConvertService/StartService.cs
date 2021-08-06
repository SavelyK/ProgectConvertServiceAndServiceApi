using ConvertService.Interfases;
using ConvertService.Models;
using Microsoft.AspNetCore.Http;
using SautinSoft.Document;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ConvertService
{
    public class StartService : IStartService
    {
        HttpClient client = new HttpClient();

        ConcurrentQueue<DocxItemModel>[] docxModels = new ConcurrentQueue<DocxItemModel>[5];
        public async Task Run()
        {
            for (int i = 0; i < 5; i++)
            {
                docxModels[i] = new ConcurrentQueue<DocxItemModel>();
            }

            Task startConvert = new Task(() => 
            {
                Task.Run(() => CreateDocxModelAsync(docxModels));
                Task.Run(() => CreateConvertAsync(docxModels, 0));
                Task.Run(() => CreateConvertAsync(docxModels, 1));
                Task.Run(() => CreateConvertAsync(docxModels, 2));
                Task.Run(() => CreateConvertAsync(docxModels, 3));
                Task.Run(() => CreateConvertAsync(docxModels, 4));
            });
                startConvert.Start();
            //Task t = Task.Run(() => CreateDocxModelAsync(docxModels));
            //Task y = Task.Run(() => CreateConvertAsync(docxModels, 0));
            //Task x = Task.Run(() => CreateConvertAsync(docxModels, 1));
            //Task v = Task.Run(() => CreateConvertAsync(docxModels, 2));
            //Task u = Task.Run(() => CreateConvertAsync(docxModels, 3));
            //Task r = Task.Run(() => CreateConvertAsync(docxModels, 4));
        }

        public async Task CreateDocxModelAsync(ConcurrentQueue<DocxItemModel>[] nameArray)
        {
            await Task.Run(async () =>
            {
                while (true)
                {
                    var response = await client.PostAsync("https://localhost:44314/api/DownloadItem?port=1000", new StringContent(string.Empty));
                    await Task.Delay(1000);
                    if (response.IsSuccessStatusCode)
                    {
                        string json = await response.Content.ReadAsStringAsync();
                        var docxFile = JsonSerializer.Deserialize<DocxItemModel>(json);
                        Console.WriteLine(docxFile.FileLength);
                        nameArray[docxFile.Priority].Enqueue(docxFile);
                    }
                    else
                    {
                        Console.WriteLine("Not Found");
                    }
                }
            });
        }
        public async Task CreateConvertAsync(ConcurrentQueue<DocxItemModel>[] nameArray, int priority)
        {
            while (true)
            {

                while (nameArray[priority].IsEmpty)
                {
                    await Task.Delay(1000);
                }
                DocxItemModel docxModel;
                if (nameArray[priority].TryDequeue(out docxModel))
                {
                    await Task.Run(async () =>
                    {

                    Console.WriteLine($"задача на конвертацию файла номер: {Task.CurrentId} сработала в потоке: {Thread.CurrentThread.ManagedThreadId}.");
                    var sw = new Stopwatch();
                    sw.Start();
                    string path = docxModel.Path;
                    byte[] fileBytes = File.ReadAllBytes(path);
                    await using (MemoryStream docxStream = new MemoryStream(fileBytes))
                    {
                        DocumentCore dc = DocumentCore.Load(docxStream, new DocxLoadOptions());
                        dc.Save(path.Replace(".docx", ".pdf"));
                    }
                    sw.Stop();
                    Console.WriteLine($"задача на конвертацию файла номер: {Task.CurrentId} закончила работу у потоке: {Thread.CurrentThread.ManagedThreadId} за время {sw.ElapsedMilliseconds} длина файла {docxModel.FileLength}");
                    });
                }
            }

        }
    }
}

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

         ConcurrentQueue<DocxItemModel>[] docxModelsArray = new ConcurrentQueue<DocxItemModel>[5];
        public async Task Run()
        {
            StartInstance();
            for (int i = 0; i < 5; i++)
            {
                docxModelsArray[i] = new ConcurrentQueue<DocxItemModel>();
            }

            Task startConvert = new Task(() =>
            {
                Task.Run(() => CreateDocxModelAsync(docxModelsArray));
                Task.Run(() => CreateConvertAsync(docxModelsArray, 0, 1));
                Task.Run(() => CreateConvertAsync(docxModelsArray, 1, 2));
                Task.Run(() => CreateConvertAsync(docxModelsArray, 2, 3));
                Task.Run(() => CreateConvertAsync(docxModelsArray, 3, 4));
                Task.Run(() => CreateConvertAsync(docxModelsArray, 4, 5));
            });
            startConvert.Start();
        }

        public async Task CreateDocxModelAsync(ConcurrentQueue<DocxItemModel>[] nameArray)
        {
            await Task.Run(async () =>
            {
                while (true)
                {

                    await Task.Delay(1000);
                    var response = await client.PostAsync("https://localhost:44314/api/DownloadItem?port=1000", new StringContent(string.Empty));
                    if (response.IsSuccessStatusCode)
                    {
                        string json = await response.Content.ReadAsStringAsync();
                        var docxFile = JsonSerializer.Deserialize<DocxItemModel>(json);
                        Console.WriteLine("file priority: " + docxFile.Priority);
                        nameArray[docxFile.Priority].Enqueue(docxFile);
                    }
                }
            });
        }
        public async Task CreateConvertAsync(ConcurrentQueue<DocxItemModel>[] nameArray, int priority, int maxCount)
        {
            int count = 0;
            while (true)
            {

                while (nameArray[priority].IsEmpty)
                {
                    await Task.Delay(100);
                }

                DocxItemModel docxModel;

                if (nameArray[priority].TryDequeue(out docxModel) | count < maxCount)
                {
                    await Task.Run(async () =>
                    {
                        count++;
                        string path = docxModel.Path;
                        byte[] fileBytes = File.ReadAllBytes(path);

                        using (MemoryStream docxStream = new MemoryStream(fileBytes))
                        {
                            DocumentCore dc = DocumentCore.Load(docxStream, new DocxLoadOptions());
                            dc.Save(path.Replace(".docx", ".pdf"));
                        }

                        var response = await client.GetAsync($"https://localhost:44314/api/StatusChange/{docxModel.Id}");
                        Console.WriteLine(response);
                        count--;

                    });
                }
            }

        }
        public void StartInstance()
        {
            Console.WriteLine("start convert service");
            var response =  client.PostAsync("https://localhost:44314/api/StartInstance/1000", new StringContent(string.Empty));
            Console.WriteLine(response);
        }
    }
}

using ConvertService.Interfases;
using ConvertService.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
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

        public async Task Run()
        {

            Task t = Task.Run(() => CreateDocxModelAsync());

        }

        public async Task CreateDocxModelAsync()
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
                    }
                    else
                    {
                        Console.WriteLine("Not Found");
                    }

                }
            });

        }
    }
}

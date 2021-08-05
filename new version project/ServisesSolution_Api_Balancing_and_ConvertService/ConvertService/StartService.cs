using ConvertService.Interfases;
using ConvertService.Model;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ConvertService
{
    public class StartService : IStartService
    {

        public async Task Run()
        {

            HttpClient client = new HttpClient();
            var response = await client.PostAsync("https://localhost:44314/api/DownloadItem?port=1000", new StringContent(string.Empty));
            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                var docxFile = JsonSerializer.Deserialize<DocxItemModel>(json);
                Console.WriteLine(docxFile.FileLength);
            }
            else
                Console.WriteLine("Not Found");



            //var answer = GetRequestAsync("https://localhost:44314/api/DownloadItem?port=1000", Params).Result;
            //string json = answer.Content.ReadAsStringAsync().Result;
            //DocxItemModel docxFile = System.Text.Json.JsonSerializer.Deserialize<DocxItemModel>(json);
            //object a = JsonConvert.DeserializeObject(json);
            //string b = JsonConvert.SerializeObject(a, Formatting.Indented);
            // Console.WriteLine(answer.Content.ReadAsStringAsync().Result);
            //Console.WriteLine(docxFile.Id);

        }
        //static async Task<HttpResponseMessage> GetRequestAsync(string address, Dictionary<string,string> Params)
        //{
        //    HttpClient client = new HttpClient();
        //    try
        //    {
        //        Uri uri = new Uri(address);
        //        var content = new FormUrlEncodedContent(Params);
        //        return await client.PostAsync(address, content);
        //    }
        //    catch (Exception x)
        //    {

        //        Console.WriteLine("Error! " + x.ToString());
        //    }
        //    finally
        //    {
        //        client.Dispose();
        //    }
        //    return null;
        //}
        //    static async Task<HttpResponseMessage> GetDocxItemAsync(string address, Dictionary<string, string> Params)
        //    {
        //        HttpClient client = new HttpClient();
        //        try
        //        {
        //            Uri uri = new Uri(address);
        //            var content = new FormUrlEncodedContent(Params);
        //            var answer = await client.PostAsync(address, content);
        //            string json = answer.Content.ReadAsStringAsync().Result;
        //            DocxItemModel docxFile = new DocxItemModel();

        //            docxFile = JsonConvert.DeserializeObject(json);
        //        }
        //        catch (Exception x)
        //        {

        //            Console.WriteLine("Error! " + x.ToString());
        //        }
        //        finally
        //        {
        //            client.Dispose();
        //        }
        //        return null;
        //    }
        //}
    }
}

using ConvertService.Interfases;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ConvertService
{
    public class StartService : IStartService
    {
       
        public async Task Run()
        { 
        Dictionary<string, string> Params = new Dictionary<string, string>()  
        {
           {"", "" },
        };

            var answer = GetRequestAsync("https://localhost:44314/api/DownloadItem?port=1000", Params).Result;
            string json = answer.Content.ReadAsStringAsync().Result;
            object a = JsonConvert.DeserializeObject(json);
            string b = JsonConvert.SerializeObject(a, Formatting.Indented);
            // Console.WriteLine(answer.Content.ReadAsStringAsync().Result);
            Console.WriteLine(b);

        }
            static async Task<HttpResponseMessage> GetRequestAsync(string address, Dictionary<string,string> Params)
            {
                HttpClient client = new HttpClient();
                try
                {
                    Uri uri = new Uri(address);
                    var content = new FormUrlEncodedContent(Params);
                    return await client.PostAsync(address, content);
                }
                catch (Exception x)
                {

                    Console.WriteLine("Error! " + x.ToString());
                }
                finally
                {
                    client.Dispose();
                }
                return null;
            }
    }
}
//"http://localhost:10384"
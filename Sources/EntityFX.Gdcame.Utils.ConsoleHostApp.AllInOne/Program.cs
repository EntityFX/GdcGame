using System;
using System.Net.Http;
using Microsoft.Owin.Hosting;

namespace EntityFX.Gdcame.Utils.ConsoleHostApp.AllInOne
{
    internal class Program
    {
        private static void Main()
        {
            var baseAddress = "http://localhost:9001/";

            // Start OWIN host 
            using (WebApp.Start<Startup>(baseAddress))
            {
                var client = new HttpClient();

                var response = client.GetAsync(baseAddress + "api/heartbeat").Result;

                Console.WriteLine(response);
                Console.WriteLine(response.Content.ReadAsStringAsync().Result);
                Console.ReadLine();
            }
        }
    }
}
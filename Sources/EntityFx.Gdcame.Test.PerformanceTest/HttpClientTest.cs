using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;

namespace EntityFx.Gdcame.Test.PerformanceTest
{
    class AuthTokenResult
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
        public string TokenType { get; set; }
        public int ExpiresIn { get; set; }
    }

    [TestClass]
    public class HttpClientTest
    {
        [TestMethod]
        public void TestConnections()
        {
            var adminToken = PostAuthAdmin().Result;
            var connectionsList = new AuthTokenResult[1000];
            for (int conn = 0; conn < 1000; conn++)
            {
                Register(adminToken, conn);
                connectionsList[conn]= PostAuth(conn).Result;
            }
            Array.ForEach(new [] {1, 10, 100}, connectionsNumber =>
            {
                Debug.WriteLine("Connections number: {0}", connectionsNumber);

                var sw = Stopwatch.StartNew();
                for (int i = 0; i < connectionsNumber; i++)
                {
                    var x = GetData(connectionsList[i]);
                }
                Debug.WriteLine("Single: {0}", sw.Elapsed);
                Debug.WriteLine("");
            });

            Array.ForEach(new[] { 1, 10, 100, 1000 }, connectionsNumber =>
            {
                Debug.WriteLine("Connections number: {0}", connectionsNumber);
                var sw = Stopwatch.StartNew();
                List<Task> taskList = new List<Task>();
                for (var i = 0; i < connectionsNumber; ++i)
                {
                    taskList.Add(new Task(() => { var x = GetData(connectionsList[i]); }));
                }
                Task.WhenAll(taskList);
                Debug.WriteLine("Parallel: {0}", sw.Elapsed);
                Debug.WriteLine("");
            });

        }

        private async static Task<AuthTokenResult> PostAuthAdmin()
        {
            var http = new HttpClient();
            http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var result = await http.PostAsync("http://localhost:9001/token", new FormUrlEncodedContent(
                new Dictionary<string, string>
                {
                    {"grant_type", "password"},
                    {"username", "entityfx"},
                    {"password", "!Biohazard1989"},

                }));
            var jsonString = await result.Content.ReadAsStringAsync();

            return await Task.Run(() => JsonConvert.DeserializeObject<AuthTokenResult>(jsonString));
        }

        private async static Task<AuthTokenResult> PostAuth(int index)
        {
            var http = new HttpClient();
            http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var result = await http.PostAsync("http://localhost:9001/token", new FormUrlEncodedContent(
                new Dictionary<string, string>
                {
                    {"grant_type", "userok-" + index},
                    {"username", "userok-" + index},
                    {"password", "!Biohazard1989"},

                }));
            var jsonString = await result.Content.ReadAsStringAsync();

            return await Task.Run(() => JsonConvert.DeserializeObject<AuthTokenResult>(jsonString));
        }

        private static object GetData(AuthTokenResult token)
        {
            var http = new HttpClient();
            http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);
            var result = JsonConvert.DeserializeObject(http.GetAsync("http://localhost:9001/api/game/game-data").Result.Content.ReadAsStringAsync().Result);
            return result;
        }

        private async static void Register(AuthTokenResult token, int index)
        {
            
            var http = new HttpClient();
            http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);
            //http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var result = await http.PostAsync("http://localhost:9001/api/auth/register", new FormUrlEncodedContent(
                new Dictionary<string, string>
                {
                    {"Login", "userok-" + index},
                    {"Password", "!Biohazard1989"},
                    {"ConfirmPAssword", "!Biohazard1989"},

                }));
            await result.Content.ReadAsStringAsync();
        }
    }
}

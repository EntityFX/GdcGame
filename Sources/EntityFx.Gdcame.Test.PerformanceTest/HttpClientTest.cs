using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using EntityFX.Gdcame.Common.Application.Model;
using EntityFX.Gdcame.Infrastructure;
using EntityFX.Gdcame.Infrastructure.Api.Auth;
using EntityFX.Gdcame.Utils.WebApiClient;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Newtonsoft.Json;
using RestSharp.Authenticators;

namespace EntityFx.Gdcame.Test.Unit
{

    [TestClass]
    public class HttpClientTest
    {
        [TestMethod]
        public void TestConnections()
        {
            var p = new RestsharpPasswordAuthProvider(new Uri("http://localhost:9001/"));
            var adminToken = p.Login(new PasswordAuthRequest()
            {
                RequestData = new PasswordAuthRequestData() { Password = "P@ssw0rd", Usename = "admin" }
            }).Result;

            var connectionsList = new IAuthContext<IAuthenticator>[10000];
            for (int conn = 0; conn < 10000; conn++)
            {
                try
                {
                    var registerRes = Register(adminToken, conn).Result;
                }
                catch (Exception ex)
                {
                    Debug.Write(ex.Message);
                }

                connectionsList[conn] = PostAuth(conn).Result;
            }

            Array.ForEach(new[] { 1, 10, 100, 1000, 10000 }, connectionsNumber =>
             {
                 Debug.WriteLine("Connections number: {0}", connectionsNumber);

                 var sw = Stopwatch.StartNew();
                 for (int i = 0; i < connectionsNumber; i++)
                 {
                     var x = GetData(connectionsList[i]).Result;
                 }

                 Debug.WriteLine("Single: {0}", sw.Elapsed);
                 Debug.WriteLine(string.Empty);
             });

            Array.ForEach(new[] { 1, 10, 100, 1000, 10000 }, connectionsNumber =>
            {
                Debug.WriteLine("Connections number: {0}", connectionsNumber);
                var sw = Stopwatch.StartNew();
                List<Task> taskList = new List<Task>();
                for (var i = 0; i < connectionsNumber; ++i)
                {
                    taskList.Add(GetData(connectionsList[i]));
                }

                Task.WhenAll(taskList);
                Debug.WriteLine("Parallel: {0}", sw.Elapsed);
                Debug.WriteLine(string.Empty);
            });

        }


        private async static Task<IAuthContext<IAuthenticator>> PostAuth(int index)
        {
            var p = new RestsharpPasswordAuthProvider(new Uri("http://localhost:9001/"));
            return await p.Login(new PasswordAuthRequest
            {
                RequestData = new PasswordAuthRequestData() { Password = "P@ssw0rd", Usename = "userok" + index }
            });

        }

        private static async Task<GameDataModel> GetData(IAuthContext<IAuthenticator> token)
        {
            var apiFactory = new RestsharpApiClientFactory();
            var gameClient = new GameApiClient(apiFactory.Build(token));
            return await gameClient.GetGameDataAsync();
        }

        private async static Task<object> Register(IAuthContext<IAuthenticator> token, int index)
        {

            var apiFactory = new RestsharpApiClientFactory();
            token.BaseUri = new Uri("http://localhost:9001");
            var authApi = new AuthApiClient(apiFactory.Build(token));


            return await authApi.Register(new RegisterAccountModel()
            {
                Login = "userok" + index,
                Password = "P@ssw0rd",
                ConfirmPassword = "P@ssw0rd"
            });

        }
    }
}

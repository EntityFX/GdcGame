using System;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using EntityFX.Gdcame.Application.Contract.Controller;
using EntityFX.Gdcame.Infrastructure.Api.Auth;
using EntityFX.Gdcame.Infrastructure.Api.Exceptions;
using EntityFX.Gdcame.Utils.Shared;
using EntityFX.Gdcame.Utils.WebApiClient;
using Newtonsoft.Json;

namespace EntityFX.Gdcame.Presentation.WebApiConsoleClient
{
    internal class ApiHelper
    {
        public static async Task<Tuple<PasswordOAuthContext, string>> UserLogin(string[] serversList, int port, string userName, string password)
        {
            var serverInfoUrl = GetApiServerUri(serversList, userName, port);

            var p = new PasswordAuthProvider(serverInfoUrl);
            var res = await p.Login(new PasswordAuthRequest<PasswordAuthData>()
            {
                RequestData = new PasswordAuthData() { Password = password, Usename = userName }
            });
            return new Tuple<PasswordOAuthContext, string>(res, userName);
        }

        public static ErrorCodes? UserLogout(PasswordOAuthContext session)
        {
            var authApi = new AuthApiClient(session);
            try
            {
                var result = authApi.Logout().Result;
            }
            catch (AggregateException loginException)
            {
                return Program.GameRunner.HandleClientException(loginException.InnerException as IClientException<ErrorData>);
            }
            return null;
        }

        public static Uri GetApiServerUri(string[] serversList, string login, int port)
        {
            var hasher = new HashHelper();
            var serverNumber = hasher.GetModuloOfUserIdHash(hasher.GetHashedString(login), serversList.Length);
            return new Uri(string.Format("http://{0}:{1}/", serversList[serverNumber], port));
        }

        public static string[] GetServers()
        {
            if (!File.Exists("servers.json"))
            {
                return null;
            }
            // deserialize JSON directly from a file
            using (StreamReader file = File.OpenText("servers.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.TypeNameHandling = TypeNameHandling.Auto;
                return (string[])serializer.Deserialize(file, typeof(string[]));
            }
        }

        public static IGameApiController GetGameClient(PasswordOAuthContext session)
        {
            return new GameApiClient(session);
        }

        public static IAdminController GetAdminClient(PasswordOAuthContext session)
        {
            return new AdminApiClient(session);
        }

        public static IServerController GetServerInfoClient(PasswordOAuthContext session)
        {
            return new ServerInfoClient(session);
        }

        public static IRatingController GetRatingClient(PasswordOAuthContext session)
        {
            return new RatingApiClient(session);
        }
    }
}
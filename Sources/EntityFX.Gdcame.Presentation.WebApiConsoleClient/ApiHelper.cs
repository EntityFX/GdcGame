﻿using System;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using EntityFX.Gdcame.Application.Contract.Controller;
using EntityFX.Gdcame.Application.Contract.Controller.Common;
using EntityFX.Gdcame.Application.Contract.Controller.MainServer;
using EntityFX.Gdcame.Infrastructure.Api.Auth;
using EntityFX.Gdcame.Infrastructure.Api.Exceptions;
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
           // var hasher = new HashHelper();
            //TODO: Use Rendezvous Hashing algorithm.
            //            var serverNumber = hasher.GetModuloOfUserIdHash(hasher.GetHashedString(login), serversList.Length);
            //todo: use not hsshed string
            //var serverNumber = hasher.GetServerNumberByRendezvousHashing(hasher.GetHashedString(login));
            //return new Uri(string.Format("http://{0}:{1}/", serversList[serverNumber], port));
            return null;
        }

        public static string[] GetServers()
        {
            return GetServers("servers.json");
        }

        public static string[] GetServers(string fileName)
        {
            if (!File.Exists(fileName))
            {
                return null;
            }
            // deserialize JSON directly from a file
            using (StreamReader file = File.OpenText(fileName))
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
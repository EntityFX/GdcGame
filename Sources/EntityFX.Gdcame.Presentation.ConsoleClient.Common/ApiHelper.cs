using System;
using System.IO;
using System.Threading.Tasks;
using EntityFx.GdCame.Test.Shared;
using EntityFX.Gdcame.Application.Contract.Controller.Common;
using EntityFX.Gdcame.Application.Contract.Controller.MainServer;
using EntityFX.Gdcame.Infrastructure.Api.Auth;
using EntityFX.Gdcame.Infrastructure.Api.Exceptions;
using EntityFX.Gdcame.Utils.Hashing;
using EntityFX.Gdcame.Utils.WebApiClient;
using Newtonsoft.Json;
using RestSharp.Authenticators;

namespace EntityFX.Gdcame.Presentation.ConsoleClient.Common
{
    using System.Linq;

    using EntityFX.Gdcame.Common.Application.Model;

    public class ApiHelper
    {
        public static async Task<PasswordOAuthContext> LoginServer(Uri server, string userName, string password)
        {
            var p = new PasswordAuthProvider(server);
            return await p.Login(new PasswordAuthRequest<PasswordAuthData>()
            {
                RequestData = new PasswordAuthData { Password = password, Usename = userName }
            });
        }

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
                return HandleClientException(loginException.InnerException as IClientException<ErrorData>);
            }
            return null;
        }

        public static Uri GetApiServerUri(string[] serversList, string login, int port)
        {
            var hasher = new HashHelper();
            var serverNumber = hasher.GetServerNumberByUserId(serversList, hasher.GetHashedString(login));
            return new Uri(string.Format("http://{0}:{1}/", serversList[serverNumber], port));
        }

        public static string[] GetServers(Uri mainServer)
        {
            return
                (new ServerInfoClient(new PasswordOAuthContext() {BaseUri = mainServer})).GetServersInfo()
                    .Result.ServerList;
        }

        public static Uri[] GetServersUri(string[] servers, int port)
        {
            return servers.Select(server => new Uri(string.Format("http://{0}:{1}/", server, port))).ToArray();
        }

        public static IGameApiController GetGameClient(PasswordOAuthContext session)
        {
            return new GameApiClient(session);
        }

        public static IAdminController GetAdminClient(PasswordOAuthContext session)
        {
            return new AdminApiClient(session);
        }

        public static IStatisticsInfo<TModel> GetStatisticsClient<TModel>(PasswordOAuthContext session)
            where TModel : ServerStatisticsInfoModel
        {
            return new StatisticsApiClient<TModel>(session);
        }

        public static IRatingController GetRatingClient(IAuthContext<IAuthenticator> session)
        {
            return new RatingApiClient(session);
        }

        public static IServerController GetServerInfoClient(IAuthContext<IAuthenticator> session)
        {
            return new ServerInfoClient(session);
        }

        public static ErrorCodes HandleClientException(IClientException<ErrorData> exception)
        {
            Console.Clear();
            var res = HandleClientExceptionErrorData(exception);
            PrettyConsole.WriteLineColor(ConsoleColor.Red, "Error: {0}", res.Item2);
            return res.Item1;
        }

        public static Tuple<ErrorCodes, string> HandleClientExceptionErrorData<T>(IClientException<T> exception)
            where T : ErrorData
        {
            ErrorCodes errorCodes = ErrorCodes.OtherError;
            string errorData = string.Empty;
            if (exception.ErrorData != null)
            {
                errorData = exception.ErrorData.Message;
                var authException = exception as ClientException<WrongAuthData<PasswordAuthData>>;
                if (authException != null)
                {
                    errorCodes = ErrorCodes.AuthError;
                    errorData += string.Format(" Login {0} not exists or wrong password",
                        authException.ErrorData.RequestData.Usename);
                }

                var validationData = exception.ErrorData as ValidationErrorData;
                if (validationData != null)
                {
                    errorCodes = ErrorCodes.ValidationError;
                    errorData += validationData.ModelState;
                }

                var invalidSessionException = exception.ErrorData as InvalidSessionException;
                if (invalidSessionException != null)
                {
                    errorCodes = ErrorCodes.InvalidSessionError;
                    errorData += invalidSessionException.ErrorData.SessionGuid;
                }

                var clientExceptionWithServerErrorData = exception.ErrorData as ClientException<ServerErrorData>;
                if (clientExceptionWithServerErrorData != null)
                {
                    errorCodes = ErrorCodes.ServerError;
                    errorData += clientExceptionWithServerErrorData.ErrorData.StackTrace;
                }

                var noConnectionException = exception as NoServerConnectionData;
                if (noConnectionException != null)
                {
                    errorCodes = ErrorCodes.ConnectionError;
                    errorData += string.Format(" Uri: {0}", noConnectionException.Uri);
                    errorData += noConnectionException.Message;
                }

                var gameFrozenException = exception.ErrorData as GameFrozenErrorData;
                if (gameFrozenException != null)
                {
                    errorCodes = ErrorCodes.OtherError;
                    errorData += string.Format(" Frozen. Try from server: {0}", gameFrozenException.Server);
                    errorData += gameFrozenException.Message;
                }
            }
            return Tuple.Create(errorCodes, errorData);
        }
    }
}
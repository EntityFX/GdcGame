﻿using System;
using System.IO;
using System.Threading.Tasks;
using EntityFx.GdCame.Presentation.Shared;
using EntityFX.Gdcame.Application.Contract.Controller.Common;
using EntityFX.Gdcame.Application.Contract.Controller.MainServer;
using EntityFX.Gdcame.Infrastructure.Api;
using EntityFX.Gdcame.Infrastructure.Api.Auth;
using EntityFX.Gdcame.Infrastructure.Api.Exceptions;
using EntityFX.Gdcame.Infrastructure.Common;
using EntityFX.Gdcame.Utils.Hashing;
using EntityFX.Gdcame.Utils.WebApiClient;

namespace EntityFX.Gdcame.Presentation.ConsoleClient.Common
{
    using System.Linq;

    using EntityFX.Gdcame.Common.Application.Model;

    public class ApiHelper<TAuthContext>
        where TAuthContext : class
    {
        private readonly ILogger _logger;
        private readonly IAuthProviderFactory<PasswordAuthRequestData, TAuthContext> _authProvider;
        private readonly IApiClientFactory<TAuthContext> _apiClientFactory;

        public ApiHelper(ILogger logger, IAuthProviderFactory<PasswordAuthRequestData, TAuthContext> authProvider, IApiClientFactory<TAuthContext> apiClientFactory)
        {
            _logger = logger;
            _authProvider = authProvider;
            _apiClientFactory = apiClientFactory;
        }

        public async Task<IAuthContext<TAuthContext>> LoginServer(Uri server, PasswordAuthRequestData authData)
        {
            //var p = new RestsharpPasswordOAuth2Provider(server);
            var p = _authProvider.Build(server);
            return await p.Login(new PasswordAuthRequest()
            {
                RequestData = authData
            });
        }

        public async Task<Tuple<IAuthContext<TAuthContext>, string>> UserLogin(string[] serversList, int port, PasswordAuthRequestData authData)
        {
            var serverInfoUrl = GetApiServerUri(serversList, authData.Usename, port);

            var p = _authProvider.Build(serverInfoUrl);
            var res = await p.Login(new PasswordAuthRequest()
            {
                RequestData = authData
            });
            return new Tuple<IAuthContext<TAuthContext>, string>(res, authData.Usename);
        }

        public ErrorCodes? UserLogout(IAuthContext<TAuthContext> session)
        {
            var authApi = new AuthApiClient(_apiClientFactory.Build(session));
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

        public Uri GetApiServerUri(string[] serversList, string login, int port)
        {
            var hasher = new HashHelper();
            var serverNumber = hasher.GetServerNumberByUserId(serversList, hasher.GetHashedString(login));
            return new Uri(string.Format("http://{0}:{1}/", serversList[serverNumber], port));
        }

        public string[] GetServers(Uri mainServer)
        {
            var auth = new AnonymousAuthContext<TAuthContext> {BaseUri = mainServer};
            try
            {
                return
                    new ServerInfoClient(_apiClientFactory.Build(auth)).GetServersInfo()
                        .Result.ServerList;
            }
            catch (Exception e)
            {
                this.HandleException(e);
                throw;
            }
        }

        public Uri[] GetServersUri(string[] servers, int port)
        {
            return servers.Select(server => new Uri(string.Format("http://{0}:{1}/", server, port))).ToArray();
        }

        public IGameApiController GetGameClient(IAuthContext<TAuthContext> session)
        {
            return new GameApiClient(_apiClientFactory.Build(session));
        }

        public IAdminController GetAdminClient(IAuthContext<TAuthContext> session)
        {
            return new AdminApiClient(_apiClientFactory.Build(session));
        }

        public IStatisticsInfo<TModel> GetStatisticsClient<TModel>(IAuthContext<TAuthContext> session)
            where TModel : ServerStatisticsInfoModel
        {
            return new StatisticsApiClient<TModel>(_apiClientFactory.Build(session));
        }

        public IRatingController GetRatingClient(IAuthContext<TAuthContext> session)
        {
            return new RatingApiClient(_apiClientFactory.Build(session));
        }

        public IServerController GetServerInfoClient(IAuthContext<TAuthContext> session)
        {
            var gameClient = new GameApiClient(_apiClientFactory.Build(session));
            return new ServerInfoClient(gameClient);
        }

        public void HandleException(Exception exception)
        {
            Console.Clear();

            if (exception is IClientException<ErrorData>)
            {
                HandleClientException(exception as IClientException<ErrorData>);
            }
            else
            {
                PrettyConsole.WriteLineColor(ConsoleColor.Red, "Error: {0}", exception.Message);
            }
            _logger.Error(exception);
        }

        public ErrorCodes HandleClientException(IClientException<ErrorData> exception)
        {
            Console.Clear();
            var res = HandleClientExceptionErrorData(exception);
            PrettyConsole.WriteLineColor(ConsoleColor.Red, "Error: {0}", res.Item2);
            _logger.Error(exception as Exception);
            return res.Item1;
        }

        public Tuple<ErrorCodes, string> HandleClientExceptionErrorData<T>(IClientException<T> exception)
            where T : ErrorData
        {
            ErrorCodes errorCodes = ErrorCodes.OtherError;
            string errorData = string.Empty;
            if (exception.ErrorData != null)
            {
                errorData = exception.ErrorData.Message;
                var authException = exception as ClientException<WrongAuthData<PasswordAuthRequestData>>;
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
            _logger.Error(exception as Exception);
            return Tuple.Create(errorCodes, errorData);
        }
    }
}
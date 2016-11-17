using System.Collections.Generic;
using System.Threading.Tasks;
using EntityFX.Gdcame.Utils.WebApiClient.Auth;
using System;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Net;
using System.Net.Http;
using RestSharp;
using RestSharp.Authenticators;


namespace EntityFX.Gdcame.Utils.WebApiClient
{
    public abstract class ApiClientBase
    {
        private readonly IAuthContext<IAuthenticator> _authContext;


        private IAuthContext<IAuthenticator> AuthContext
        {
            get { return _authContext; }
        }

        public int Timeout { get; private set; }

        protected ApiClientBase(IAuthContext<IAuthenticator> authContext, int? timeout)
        {
            _authContext = authContext;
            Timeout = timeout != null ? (int)timeout : 60000;
        }

        protected async Task<IRestResponse<TModel>> ExecuteRequestAsync<TModel>(string requestUriPath, Method method = Method.GET, IEnumerable<Parameter> parameters = null)
        {
            var clientFactory = new GameClientFactory(AuthContext.BaseUri);
            var request = clientFactory.CreateRequest(requestUriPath);

            if (parameters != null)
            {
                if (parameters.Any(_ => string.IsNullOrEmpty(_.Name)))
                {
                    request.AddJsonBody(parameters.FirstOrDefault(_ => string.IsNullOrEmpty(_.Name)).Value);
                }
                else
                {
                    request.RequestFormat = DataFormat.Json;
                    foreach (var parameter in parameters)
                    {
                        request.AddParameter(parameter);
                        //request.Parameters.Add(parameter);
                    }
                }



            }


            request.Method = method;
            var client = clientFactory.CreateClient();

            client.Timeout = Timeout;
            client.AddHandler("application/json", CustomJsonDeserializer.Default);
            client.AddHandler("text/javascript", CustomJsonDeserializer.Default);
            client.Authenticator = AuthContext.Context;
            //client.IgnoreResponseStatusCode = true;
            try
            {
                var res = await client.ExecuteTaskAsync<TModel>(request);
                if (
                    new HttpStatusCode[]
                    {
                        HttpStatusCode.BadRequest, HttpStatusCode.Forbidden, HttpStatusCode.Unauthorized, HttpStatusCode.BadGateway, HttpStatusCode.InternalServerError

                    }.Contains(res.StatusCode) || res.ResponseStatus == ResponseStatus.Error)
                {
                    ExceptionHandlerHelper.HandleNotSuccessRequest(res);
                }
                return res;
            }
            catch (Exception exception)
            {
                ExceptionHandlerHelper.HandleHttpRequestException(exception);
            }
            return null;

        }

        protected async Task<IRestResponse<TResponse>> ExecuteRequestAsync<TRequest, TResponse>(string requestUriPath, Method method = Method.GET, TRequest data = null)
            where TRequest : class
        {
            var clientFactory = new GameClientFactory(AuthContext.BaseUri);
            var request = clientFactory.CreateRequest(requestUriPath);
            request.RequestFormat = DataFormat.Json;
            if (data != null)
            {
                request.AddBody(data);
            }

            request.Method = method;
            var client = clientFactory.CreateClient();

            client.Timeout = Timeout;
            client.AddHandler("application/json", CustomJsonDeserializer.Default);
            client.AddHandler("text/javascript", CustomJsonDeserializer.Default);
            client.Authenticator = AuthContext.Context;


            try
            {
                var res = await client.ExecuteTaskAsync<TResponse>(request);
                if (
                    new HttpStatusCode[]
                    {
                        HttpStatusCode.BadRequest, HttpStatusCode.Forbidden, HttpStatusCode.Unauthorized, HttpStatusCode.BadGateway, HttpStatusCode.InternalServerError

                    }.Contains(res.StatusCode))
                {
                    ExceptionHandlerHelper.HandleNotSuccessRequest(res);
                }
                return res;
            }
            catch (HttpRequestException exception)
            {
                ExceptionHandlerHelper.HandleHttpRequestException(exception);
            }
            return null;
        }
    }

}

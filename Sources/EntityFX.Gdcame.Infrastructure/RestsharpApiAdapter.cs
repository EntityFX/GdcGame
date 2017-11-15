using EntityFX.Gdcame.Infrastructure.Api;
using EntityFX.Gdcame.Infrastructure.Api.Auth;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EntityFX.Gdcame.Infrastructure
{
    public class RestsharpApiAdapter : IApiClient
    {
        private readonly IAuthContext<IAuthenticator> _authContext;

        public RestsharpApiAdapter(IAuthContext<IAuthenticator> authContext)
        {
            _authContext = authContext;
        }

        private IAuthContext<IAuthenticator> AuthContext
        {
            get { return _authContext; }
        }

        public int Timeout { get; private set; }

        public async Task<IApiResponse<TModel>> ExecuteRequestAsync<TModel>(string requestUriPath, ApiRequestMethod method = ApiRequestMethod.GET, IEnumerable<ApiParameter> parameters = null)
        {
            var res = await ExecuteRequestAsyncImplementation<TModel>(requestUriPath, (Method)(int)method, parameters != null ? parameters.Select(p => new Parameter() { Name = p.Name, Value = p.Value, ContentType = p.ContentType }): null);
            return new ApiResponse<TModel>() { Data = res.Data, HttpCode = res.StatusCode };
        }

        public async Task<IApiResponse<TResponse>> ExecuteRequestAsync<TRequest, TResponse>(string requestUriPath, ApiRequestMethod method = ApiRequestMethod.GET, TRequest data = default(TRequest))
            where TRequest : class
        {
            var res = await ExecuteRequestAsyncImplementation<TRequest, TResponse>(requestUriPath, (Method)(int)method, data);
            return new ApiResponse<TResponse>() { Data = res.Data, HttpCode = res.StatusCode };
        }

        protected async Task<IRestResponse<TModel>> ExecuteRequestAsyncImplementation<TModel>(string requestUriPath, Method method = Method.GET, IEnumerable<Parameter> parameters = null)
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
            client.Authenticator = GetAuthenticator();
            //client.IgnoreResponseStatusCode = true;
            IRestResponse<TModel> res = null;
            try
            {
                res = await client.ExecuteTaskAsync<TModel>(request);
            }
            catch (Exception exception)
            {
                ExceptionHandlerHelper.HandleHttpRequestException(exception);
            }

            if (
                new HttpStatusCode[]
                {
                        HttpStatusCode.BadRequest, HttpStatusCode.Forbidden, HttpStatusCode.Unauthorized, HttpStatusCode.BadGateway, HttpStatusCode.InternalServerError, HttpStatusCode.NotFound,   HttpStatusCode.MovedPermanently

                }.Contains(res.StatusCode) || res.ResponseStatus == ResponseStatus.Error)
            {
                ExceptionHandlerHelper.HandleNotSuccessRequest(res);
            }



            return res;

        }

        protected async Task<IRestResponse<TResponse>> ExecuteRequestAsyncImplementation<TRequest, TResponse>(string requestUriPath, Method method = Method.GET, TRequest data = null)
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
            client.Authenticator = GetAuthenticator();


            try
            {
                var res = await client.ExecuteTaskAsync<TResponse>(request);
                if (
                    new HttpStatusCode[]
                    {
                        HttpStatusCode.BadRequest, HttpStatusCode.Forbidden, HttpStatusCode.Unauthorized, HttpStatusCode.BadGateway, HttpStatusCode.InternalServerError, HttpStatusCode.NotFound

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

        private IAuthenticator GetAuthenticator()
        {
            if (AuthContext is AnonymousAuthContext<IAuthenticator>)
            {
                return null;
            }

            if (AuthContext is PasswordOAuth2Context<IAuthenticator>)
            {
                return AuthContext.Context.ApiContext;
            }
            return null;
        }
    }
}

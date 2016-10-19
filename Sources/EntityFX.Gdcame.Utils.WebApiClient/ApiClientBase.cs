﻿using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using EntityFX.Gdcame.Utils.WebApiClient.Auth;
using RestSharp.Portable;
using System;

namespace EntityFX.Gdcame.Utils.WebApiClient
{
    public abstract class ApiClientBase
    {
        private readonly IAuthContext<IAuthenticator> _authContext;


        private IAuthContext<IAuthenticator> AuthContext
        {
            get { return _authContext; }
        }

        public TimeSpan Timeout { get; private set; }

        protected ApiClientBase(IAuthContext<IAuthenticator> authContext, TimeSpan? timeout)
        {
            _authContext = authContext;
            Timeout = timeout != null ? (TimeSpan)timeout : TimeSpan.FromSeconds(60);
        }
        
        protected async Task<IRestResponse<TModel>> ExecuteRequestAsync<TModel>(string requestUriPath, Method method = Method.GET, IEnumerable<Parameter> parameters = null)
        {
            var clientFactory = new GameClientFactory(AuthContext.BaseUri);
            var request = clientFactory.CreateRequest(requestUriPath);
            if (parameters != null)
            {
                foreach (var parameter in parameters)
                {
                    request.Parameters.Add(parameter);
                }
            }

            request.Method = method;
            var client = clientFactory.CreateClient();
            client.Timeout = Timeout;
            client.AddHandler("application/json", CustomJsonDeserializer.Default);
            client.AddHandler("text/javascript", CustomJsonDeserializer.Default);
            client.Authenticator = AuthContext.Context;
            client.IgnoreResponseStatusCode = true;
            try
            {
                var res = await client.Execute<TModel>(request);
                if (!res.IsSuccess)
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
            client.IgnoreResponseStatusCode = true;

            try
            {
                var res = await client.Execute<TResponse>(request);
                if (!res.IsSuccess)
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
using EntityFX.Gdcame.Application.Contract.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using EntityFX.Gdcame.Application.Contract.Model;
using EntityFX.Gdcame.Infrastructure.Api;
using EntityFX.Gdcame.Infrastructure.Api.Auth;
using RestSharp;
using RestSharp.Authenticators;


namespace EntityFX.Gdcame.Utils.WebApiClient
{
    public class ServerInfoClient : ApiClientBase, IServerController
    {
        public ServerInfoClient(int? timeout) : base(null, timeout)
        {
        }

        public ServerInfoClient(IAuthContext<IAuthenticator> authContext, int? timeout = null) : base(authContext, timeout)
        {
        }

        public async Task<ServerInfoModel> GetServersInfo()
        {
            var response = await ExecuteRequestAsync<ServerInfoModel>("/api/server-info", Method.GET);
            return response.Data;
        }

        public string Echo(string text)
        {
            var response =  ExecuteRequestAsync<string>("/api/server-info/echo", Method.GET, new List<Parameter>() { new Parameter() { Type = ParameterType.QueryString, Name = "text",  Value = text } });
            var data = response.Result;
            return data.Data;
        }

        public string EchoAuth(string text)
        {
            var response = ExecuteRequestAsync<string>("/api/server-info/echo-async", Method.GET, new List<Parameter>() { new Parameter() { Type = ParameterType.GetOrPost, Name = "text", Value = text } });
            return response.Result.Data;
        }
    }
}

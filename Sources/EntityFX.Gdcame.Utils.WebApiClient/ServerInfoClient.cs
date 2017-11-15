﻿using EntityFX.Gdcame.Application.Contract.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using EntityFX.Gdcame.Application.Contract.Controller.Common;
using EntityFX.Gdcame.Common.Application.Model;
using EntityFX.Gdcame.Infrastructure.Api;
using EntityFX.Gdcame.Infrastructure.Api.Auth;



namespace EntityFX.Gdcame.Utils.WebApiClient
{
    public class ServerInfoClient : ApiClientBase, IServerController
    {
        public ServerInfoClient(IApiClient authContext) : base(authContext)
        {
        }

        public async Task<ServerInfoModel> GetServersInfo()
        {
            var response = await ExecuteRequestAsync<ServerInfoModel>("/api/server-info", ApiRequestMethod.GET);
            return response.Data;
        }

        public string Echo(string text)
        {
            var response =  ExecuteRequestAsync<string>("/api/server-info/echo", ApiRequestMethod.GET, new List<ApiParameter>() { new ApiParameter() { Type = ApiParameterType.QueryString, Name = "text",  Value = text } });
            var data = response.Result;
            return data.Data;
        }

        public string EchoAuth(string text)
        {
            var response = ExecuteRequestAsync<string>("/api/server-info/echo-async", ApiRequestMethod.GET, new List<ApiParameter>() { new ApiParameter() { Type = ApiParameterType.QueryString, Name = "text", Value = text } });
            return response.Result.Data;
        }
    }
}

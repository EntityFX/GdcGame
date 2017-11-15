using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EntityFX.Gdcame.Application.Contract.Model;
using EntityFX.Gdcame.Application.Contract.Model.MainServer;
using EntityFX.Gdcame.Infrastructure.Api;
using EntityFX.Gdcame.Infrastructure.Api.Auth;

namespace EntityFX.Gdcame.Utils.WebApiClient
{
    public class AuthApiClient: ApiClientBase
    {

        public async Task<object> Register(RegisterAccountModel model)
        {
            var response = await ExecuteRequestAsync<RegisterAccountModel, object>("/api/auth/register", ApiRequestMethod.POST, model);
            return response.Data;
        }

        public async Task<object> Logout()
        {
            var response = await ExecuteRequestAsync<object>("/api/auth/logout", ApiRequestMethod.POST);
            return response.Data;
        }

        public AuthApiClient(IApiClient authContext) : base(authContext)
        {
        }
    }
}
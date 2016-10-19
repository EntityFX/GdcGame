using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EntityFX.Gdcame.Application.Contract.Model;
using EntityFX.Gdcame.Utils.WebApiClient.Auth;
using EntityFX.Gdcame.Utils.WebApiClient.Exceptions;
using RestSharp.Portable;

namespace EntityFX.Gdcame.Utils.WebApiClient
{
    public class AuthApiClient: ApiClientBase
    {

        public async Task<object> Register(RegisterAccountModel model)
        {
            var response = await ExecuteRequestAsync<RegisterAccountModel, object>("/api/auth/register", Method.POST, model);
            return response.Data;
        }

        public async Task<object> Logout()
        {
            var response = await ExecuteRequestAsync<object>("/api/auth/logout", Method.POST);
            return response.Data;
        }

        public AuthApiClient(IAuthContext<IAuthenticator> authContext, TimeSpan? timeout = null) : base(authContext, timeout)
        {
        }
    }
}
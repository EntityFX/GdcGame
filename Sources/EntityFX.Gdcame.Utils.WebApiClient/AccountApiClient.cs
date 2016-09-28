﻿using EntityFX.Gdcame.Presentation.Contract.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFX.Gdcame.Presentation.Contract.Model;
using EntityFX.Gdcame.Utils.WebApiClient.Auth;
using RestSharp.Portable;

namespace EntityFX.Gdcame.Utils.WebApiClient
{
    public class AccountApiClient : ApiClientBase, IAccountController
    {
        public AccountApiClient(IAuthContext<IAuthenticator> authContext) : base(authContext)
        {
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var response = await ExecuteRequestAsync<string, bool>("/api/admin/accounts", Method.DELETE, id);
            return response.Data;
        }

        public async Task<IEnumerable<AccountInfoModel>> GetAsync(string filter = null)
        {
            var response = await ExecuteRequestAsync<IEnumerable<AccountInfoModel>>("/api/admin/accounts");
            return response.Data;
        }

        public async Task<AccountInfoModel> GetByIdAsync(string id)
        {
            var response = await ExecuteRequestAsync<AccountInfoModel>(string.Format("/api/admin/accounts/", id));
            return response.Data;
        }

        public async Task<AccountInfoModel> GetByLoginAsync(string login)
        {
            var response = await ExecuteRequestAsync<AccountInfoModel>(string.Format("/api/admin/accounts/login/{0}", login));
            return response.Data;
        }
    }
}

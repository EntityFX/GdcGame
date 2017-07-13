using EntityFX.Gdcame.Application.Contract.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFX.Gdcame.Application.Contract.Controller.MainServer;
using EntityFX.Gdcame.Application.Contract.Model;
using EntityFX.Gdcame.Application.Contract.Model.MainServer;
using EntityFX.Gdcame.Infrastructure.Api;
using EntityFX.Gdcame.Infrastructure.Api.Auth;
using RestSharp;
using RestSharp.Authenticators;


namespace EntityFX.Gdcame.Utils.WebApiClient
{
    public class AccountApiClient : ApiClientBase, IAccountController
    {
        public AccountApiClient(IAuthContext<IAuthenticator> authContext, int? timeout = null) : base(authContext, timeout)
        {
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var response = await ExecuteRequestAsync<string, bool>("/api/admin/accounts", Method.DELETE, id);
            return response != null ? response.Data : false;
        }

        public async Task<IEnumerable<AccountInfoModel>> GetAsync(string filter = null)
        {
            var response = await ExecuteRequestAsync<IEnumerable<AccountInfoModel>>("/api/admin/accounts");
            return response != null ? response.Data : null;
        }

        public async Task<AccountInfoModel> GetByIdAsync(string id)
        {
            var response = await ExecuteRequestAsync<AccountInfoModel>(string.Format("/api/admin/accounts/", id));
            return response != null ? response.Data : null;
        }

        public async Task<AccountInfoModel> GetByLoginAsync(string login)
        {
            var response = await ExecuteRequestAsync<AccountInfoModel>(string.Format("/api/admin/accounts/login/{0}", login));
            return response != null ? response.Data : null;
        }
    }
}

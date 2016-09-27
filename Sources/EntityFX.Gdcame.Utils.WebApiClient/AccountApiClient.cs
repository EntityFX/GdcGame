using EntityFX.Gdcame.Presentation.Contract.Controller;
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

        public async Task DeleteAsync(string id)
        {
            var response = await ExecuteRequestAsync<string, object>("/api/admin/accounts", Method.DELETE, id);
            return;
        }

        public IEnumerable<AccountInfoModel> Get(string filter = null)
        {
            var response = ExecuteRequestAsync<IEnumerable<AccountInfoModel>>("/api/admin/accounts").Result;
            return response.Data;
        }

        public AccountInfoModel GetById(string id)
        {
            var response = ExecuteRequestAsync<AccountInfoModel>(string.Format("/api/admin/accounts/", id)).Result;
            return response.Data;
        }

        public AccountInfoModel GetByLogin(string login)
        {
            var response = ExecuteRequestAsync<AccountInfoModel>(string.Format("/api/admin/accounts/login/{0}", login)).Result;
            return response.Data;
        }
    }
}

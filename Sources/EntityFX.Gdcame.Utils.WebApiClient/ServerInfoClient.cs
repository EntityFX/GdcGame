using EntityFX.Gdcame.Application.Contract.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFX.Gdcame.Application.Contract.Model;
using EntityFX.Gdcame.Utils.WebApiClient.Auth;
using RestSharp.Portable;

namespace EntityFX.Gdcame.Utils.WebApiClient
{
    public class ServerInfoClient : ApiClientBase, IServerController
    {
        public ServerInfoClient(TimeSpan? timeout) : base(null, timeout)
        {
        }

        public ServerInfoClient(IAuthContext<IAuthenticator> authContext, TimeSpan? timeout = null) : base(authContext, timeout)
        {
        }

        public async Task<ServerInfoModel> GetServersInfo()
        {
            var response = await ExecuteRequestAsync<ServerInfoModel>("/api/server-info", Method.GET);
            return response.Data;
        }
    }
}

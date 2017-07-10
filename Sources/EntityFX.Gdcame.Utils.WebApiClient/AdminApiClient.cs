using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EntityFX.Gdcame.Application.Contract.Controller;
using EntityFX.Gdcame.Application.Contract.Model;
using EntityFX.Gdcame.Infrastructure.Api;
using EntityFX.Gdcame.Infrastructure.Api.Auth;
using RestSharp;
using RestSharp.Authenticators;

namespace EntityFX.Gdcame.Utils.WebApiClient
{
    public class AdminApiClient : ApiClientBase, IAdminController
    {
        public AdminApiClient(IAuthContext<IAuthenticator> authContext, int? timeout = null) : base(authContext, timeout)
        {
        }

        public async Task<UserSessionsModel[]> GetActiveSessions()
        {
            var response = await ExecuteRequestAsync<UserSessionsModel[]>("/api/admin/sessions");
            return response != null ? response.Data : new UserSessionsModel[] { };
        }

        public ServerStatisticsInfoModel GetStatistics()
        {
            return ExecuteRequestAsync<ServerStatisticsInfoModel>("/api/admin/statistics").Result.Data;
        }

        public string UpdateNodesList(string[] newServersList)
        {
            List <Parameter> parameters =  new List<Parameter>();
            foreach (var server in newServersList)
            {
                parameters.Add(new Parameter() { Type = ParameterType.QueryString, Name = "newServersList", Value = server });
            }
            var response = ExecuteRequestAsync<string>("/api/admin/update_nodes_list", Method.GET, parameters);
            var data = response.Result;
            return data.Data;
        }

        public async void CloseSessionByGuid(Guid guid)
        {
            await ExecuteRequestAsync<object, object>("/api/admin/sessions/guid", Method.DELETE, guid);
        }

        public async void CloseAllUserSessions(string username)
        {
            await ExecuteRequestAsync<string, object>("/api/admin/sessions/user", Method.DELETE, username);
        }

        public async void CloseAllSessions()
        {
            await ExecuteRequestAsync<object>("/api/admin/sessions/all", Method.DELETE);
        }

        public async void CloseAllSessionsExcludeThis(Guid guid)
        {
            await ExecuteRequestAsync<object, object>("/api/admin/sessions/exclude", Method.DELETE, guid);
        }

        public async void WipeUser(string username)
        {
            await ExecuteRequestAsync<string, object>("/api/admin/wipe-user", Method.POST, username);
        }

        public async void ReloadGame(string username)
        {
            await ExecuteRequestAsync<string, object>("/api/admin/reload-game", Method.POST, username);
        }

        public void StopGame(string username)
        {
            var res = ExecuteRequestAsync<string, object>("/api/admin/games/user", Method.DELETE, username).Result;
        }

        public void StopAllGames()
        {
            var res = ExecuteRequestAsync<string, object>("/api/admin/games/all", Method.DELETE).Result;
        }
    }
}
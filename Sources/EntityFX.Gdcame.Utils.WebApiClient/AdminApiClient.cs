using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EntityFX.Gdcame.Application.Contract.Controller;
using EntityFX.Gdcame.Application.Contract.Controller.MainServer;
using EntityFX.Gdcame.Application.Contract.Model;
using EntityFX.Gdcame.Application.Contract.Model.MainServer;
using EntityFX.Gdcame.Infrastructure.Api;


namespace EntityFX.Gdcame.Utils.WebApiClient
{
    public class AdminApiClient : StatisticsApiClient<MainServerStatisticsInfoModel>, IAdminController
    {
        public AdminApiClient(IApiClient authContext) : base(authContext)
        {
        }

        public async Task<UserSessionsModel[]> GetActiveSessions()
        {
            var response = await ExecuteRequestAsync<UserSessionsModel[]>("/api/admin/sessions");
            return response != null ? response.Data : new UserSessionsModel[] { };
        }

        public string UpdateNodesList(string[] newServersList)
        {
            List <ApiParameter> parameters =  new List<ApiParameter>();
            foreach (var server in newServersList)
            {
                parameters.Add(new ApiParameter() { Type = ApiParameterType.QueryString, Name = "newServersList", Value = server });
            }
            var response = ExecuteRequestAsync<string>("/api/admin/update_nodes_list", ApiRequestMethod.GET, parameters);
            var data = response.Result;
            return data.Data;
        }

        public async void CloseSessionByGuid(Guid guid)
        {
            await ExecuteRequestAsync<object, object>("/api/admin/sessions/guid", ApiRequestMethod.DELETE, guid);
        }

        public async void CloseAllUserSessions(string username)
        {
            await ExecuteRequestAsync<string, object>("/api/admin/sessions/user", ApiRequestMethod.DELETE, username);
        }

        public async void CloseAllSessions()
        {
            await ExecuteRequestAsync<object>("/api/admin/sessions/all", ApiRequestMethod.DELETE);
        }

        public async void CloseAllSessionsExcludeThis(Guid guid)
        {
            await ExecuteRequestAsync<object, object>("/api/admin/sessions/exclude", ApiRequestMethod.DELETE, guid);
        }

        public async void WipeUser(string username)
        {
            await ExecuteRequestAsync<string, object>("/api/admin/wipe-user", ApiRequestMethod.POST, username);
        }

        public async void UpdateServersList(string[] serversList)
        {
            await ExecuteRequestAsync<string[], object>("/api/admin/servers", ApiRequestMethod.POST, serversList);
        }

        public async void RemoveServer(string address)
        {
            await ExecuteRequestAsync<object, object>("/api/admin/servers", ApiRequestMethod.DELETE, address);
        }

        public async void ReloadGame(string username)
        {
            await ExecuteRequestAsync<string, object>("/api/admin/reload-game", ApiRequestMethod.POST, username);
        }

        public void StopGame(string username)
        {
            var res = ExecuteRequestAsync<string, object>("/api/admin/games/user", ApiRequestMethod.DELETE, username).Result;
        }

        public void StopAllGames()
        {
            var res = ExecuteRequestAsync<string, object>("/api/admin/games/all", ApiRequestMethod.DELETE).Result;
        }
    }
}
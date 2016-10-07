using System;
using System.Threading.Tasks;
using EntityFX.Gdcame.Application.Contract.Controller;
using EntityFX.Gdcame.Application.Contract.Model;
using EntityFX.Gdcame.Utils.WebApiClient.Auth;
using RestSharp.Portable;

namespace EntityFX.Gdcame.Utils.WebApiClient
{
    public class AdminApiClient : ApiClientBase, IAdminController
    {
        public AdminApiClient(IAuthContext<IAuthenticator> authContext) : base(authContext)
        {
        }

        public async Task<UserSessionsModel[]> GetActiveSessions()
        {
            var response = await ExecuteRequestAsync<UserSessionsModel[]>("/api/admin/sessions");
            return response.Data;
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
    }
}
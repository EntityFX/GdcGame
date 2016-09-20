using System;
using System.Threading.Tasks;
using EntityFX.Gdcame.Presentation.Contract.Model;

namespace EntityFX.Gdcame.Presentation.Contract.Controller
{
    public interface IAdminController
    {

        Task<UserSessionsModel[]> GetActiveSessions();

        void CloseSessionByGuid(Guid guid);

        void CloseAllUserSessions(string username);

        void CloseAllSessions();

        void CloseAllSessionsExcludeThis(Guid guid);

        void WipeUser(string username);

        void ReloadGame(string username);
    }
}
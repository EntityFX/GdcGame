using System;
using System.Threading.Tasks;
using EntityFX.Gdcame.Application.Contract.Model;

namespace EntityFX.Gdcame.Application.Contract.Controller
{
    public interface IAdminController
    {

        Task<UserSessionsModel[]> GetActiveSessions();

        ServerStatisticsInfoModel GetStatistics();

        void CloseSessionByGuid(Guid guid);

        void CloseAllUserSessions(string username);

        void CloseAllSessions();

        void CloseAllSessionsExcludeThis(Guid guid);

        void WipeUser(string username);

        void ReloadGame(string username);
    }
}
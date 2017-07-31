using System;
using System.Threading.Tasks;
using EntityFX.Gdcame.Application.Contract.Model;
using EntityFX.Gdcame.Application.Contract.Model.MainServer;
using EntityFX.Gdcame.Common.Application.Model;

namespace EntityFX.Gdcame.Application.Contract.Controller.MainServer
{
    public interface IAdminController
    {

        Task<UserSessionsModel[]> GetActiveSessions();

        ServerStatisticsInfoModel GetStatistics();

        void UpdateServersList(string[] newServersList);

        void CloseSessionByGuid(Guid guid);

        void CloseAllUserSessions(string username);

        void CloseAllSessions();

        void CloseAllSessionsExcludeThis(Guid guid);

        void WipeUser(string username);


        void ReloadGame(string username);

        void StopGame(string username);

        void StopAllGames();
    }
}
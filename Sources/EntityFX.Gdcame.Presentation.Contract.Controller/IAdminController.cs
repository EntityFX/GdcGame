using System;
using System.Threading.Tasks;
using EntityFX.Gdcame.Application.Contract.Model;
using EntityFX.Gdcame.Application.Contract.Model.MainServer;

namespace EntityFX.Gdcame.Application.Contract.Controller.MainServer
{
    public interface IAdminController
    {

        Task<UserSessionsModel[]> GetActiveSessions();

        ServerStatisticsInfoModel GetStatistics();

        string UpdateNodesList(string[] newServersList);

        void CloseSessionByGuid(Guid guid);

        void CloseAllUserSessions(string username);

        void CloseAllSessions();

        void CloseAllSessionsExcludeThis(Guid guid);

        void WipeUser(string username);

        void AddServer(string address);

        void RemoveServer(string address);

        void ReloadGame(string username);

        void StopGame(string username);

        void StopAllGames();
    }
}
using System;
using System.Threading.Tasks;
using EntityFX.Gdcame.Application.Contract.Model;
using EntityFX.Gdcame.Application.Contract.Model.MainServer;

namespace EntityFX.Gdcame.Application.Contract.Controller.MainServer
{
    using EntityFX.Gdcame.Application.Contract.Controller.Common;

    public interface IAdminController : IStatisticsInfo<MainServerStatisticsInfoModel>
    {

        Task<UserSessionsModel[]> GetActiveSessions();

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
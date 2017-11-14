using System;
using System.Linq;
using System.Threading.Tasks;

using EntityFX.Gdcame.Infrastructure.Common;
using EntityFX.Gdcame.Manager.Contract.Common;
using EntityFX.Gdcame.Manager.Contract.MainServer.AdminManager;

namespace EntityFX.Gdcame.Manager.MainServer
{
    using EntityFX.Gdcame.Contract.Common;
    using EntityFX.Gdcame.Contract.Common.Statistics;
    using EntityFX.Gdcame.DataAccess.Contract.Common.User;
    using EntityFX.Gdcame.Engine.Contract.GameEngine;
    using EntityFX.Gdcame.Manager.Common;
    using EntityFX.Gdcame.Manager.Contract.Common.AdminManager;

    public class AdminManager : AdminManagerBase<MainServerStatisticsInfo>, IAdminManager
    {
        private readonly IGameSessions _gameSessions;
        private readonly IPerformanceHelper _performanceHelper;
        private readonly IUserDataAccessService _userDataAccessService;
        private readonly SystemInfo _systemInfo;
        private readonly IOperationContextHelper _operationContextHelper;

        public AdminManager(IOperationContextHelper operationContextHelper
            , IGameSessions gameSessions
            , IPerformanceHelper performanceHelper
            , IUserDataAccessService userDataAccessService
            , SystemInfo systemInfo)
            : base(gameSessions, performanceHelper, systemInfo)
        {
            _gameSessions = gameSessions;
            _performanceHelper = performanceHelper;
            _userDataAccessService = userDataAccessService;
            _systemInfo = systemInfo;
            _operationContextHelper = operationContextHelper;
        }

        public UserSessionsInfo[] GetActiveSessions()
        {
            return _gameSessions.Sessions.Values.GroupBy(_ => _.Login).Select(sessionsOfUser => new UserSessionsInfo
            {
                UserName = sessionsOfUser.Key,
                UserSessions = sessionsOfUser.ToArray()
            }).ToArray();
        }

        public void CloseSessionByGuid(Guid guid)
        {
            _gameSessions.RemoveSession(guid);
        }

        public void CloseAllUserSessions(string username)
        {
            Parallel.ForEach(_gameSessions.Sessions.Values.Where(_ => _.Login == username), _ => _gameSessions.RemoveSession(_.SessionIdentifier));
        }

        public void CloseAllSessions()
        {
            var sessionId = _operationContextHelper.Instance.SessionId;
            _gameSessions.RemoveAllSessions();
        }

        public void CloseAllSessionsExcludeThis(Guid guid)
        {
            Parallel.ForEach(_gameSessions.Sessions.Values.Where(_ => _.SessionIdentifier != guid),
                _ => _gameSessions.RemoveSession(_.SessionIdentifier));
        }

        public void WipeUser(string username)
        {
            _gameSessions.GetGame(username).Reset();
        }

        public void ReloadGame(string username)
        {
            throw new NotImplementedException();
        }


        public override MainServerStatisticsInfo GetStatisticsInfo()
        {
            var statistics = base.GetStatisticsInfo();
            statistics.ActiveGamesCount = _gameSessions.Games.Count;
            statistics.RegistredUsersCount = _userDataAccessService.Count();
            statistics.PerformanceInfo = _gameSessions.PerformanceInfo;
            return statistics;
        }

        public void StopGame(string login)
        {
            _gameSessions.RemoveGame(new UserData() { Login = login });
        }

        public void StopAllGames()
        {
            _gameSessions.RemoveAllGames();
        }

    }

}
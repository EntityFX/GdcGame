using System;
using System.Linq;
using EntityFX.Gdcame.Infrastructure.Common;
using EntityFX.Gdcame.Manager.Contract.AdminManager;
using EntityFX.Gdcame.Manager.Contract.UserManager;

namespace EntityFX.Gdcame.Manager
{
    public class AdminManager : IAdminManager
    {
        private readonly GameSessions _gameSessions;
        private readonly IPerformanceHelper _performanceHelper;
        private readonly SystemInfo _systemInfo;
        private readonly IOperationContextHelper _operationContextHelper;
        private static readonly DateTime ServerStartTime = DateTime.Now;

        public AdminManager(IOperationContextHelper operationContextHelper, GameSessions gameSessions, IPerformanceHelper performanceHelper, SystemInfo systemInfo)
        {
            _gameSessions = gameSessions;
            _performanceHelper = performanceHelper;
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
            _gameSessions.Sessions.Values.Where(_ => _.Login == username).AsParallel()
                .ForAll(_ => _gameSessions.RemoveSession(_.SessionIdentifier));
        }

        public void CloseAllSessions()
        {
            var sessionId = _operationContextHelper.Instance.SessionId;
            _gameSessions.RemoveAllSessions();
        }

        public void CloseAllSessionsExcludeThis(Guid guid)
        {
            _gameSessions.Sessions.Values.Where(_ => _.SessionIdentifier != guid).AsParallel()
                .ForAll(_ => _gameSessions.RemoveSession(_.SessionIdentifier));
        }

        public void WipeUser(string username)
        {
            _gameSessions.GetGame(username).Reset();
        }

        public void ReloadGame(string username)
        {
            throw new NotImplementedException();
        }

        public StatisticsInfo GetStatisticsInfo()
        {
            return new StatisticsInfo()
            {
                ActiveSessionsCount = _gameSessions.Sessions.Count,
                ActiveGamesCount = _gameSessions.Games.Count,
                ServerStartDateTime = ServerStartTime,
                ServerUptime = DateTime.Now - ServerStartTime,
                PerformanceInfo = _gameSessions.PerformanceInfo,
                ResourcesUsageInfo = new ResourcesUsageInfo()
                {
                    CpuUsed = _performanceHelper.CpuUsage,
                    MemoryAvailable = _performanceHelper.MemoryUsage,
                    MemoryUsedByProcess = _performanceHelper.MemoryUsageByProcess
                },
                SystemInfo = _systemInfo
            };
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
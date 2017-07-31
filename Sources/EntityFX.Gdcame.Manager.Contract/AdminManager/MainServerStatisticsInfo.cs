using System.Runtime.Serialization;
using EntityFX.Gdcame.Manager.Contract.Common.Statistics;

namespace EntityFX.Gdcame.Manager.Contract.MainServer.AdminManager
{
    public class MainServerStatisticsInfo : StatisticsInfo
    {
        [DataMember]
        public int ActiveSessionsCount { get; set; }
        [DataMember]
        public int ActiveGamesCount { get; set; }
        [DataMember]
        public int RegistredUsersCount { get; set; }
    }
}
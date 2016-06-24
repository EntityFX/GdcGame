using System;
using EntityFX.EconomicsArcade.Contract.Common;
using EntityFX.EconomicsArcade.Contract.Common.Counters;
using EntityFX.EconomicsArcade.Contract.Manager.GameManager;
using EntityFX.EconomicsArcade.Infrastructure.Service;

namespace EntityFX.EconomicsArcade.Utils.ClientProxy.Manager
{
    public class GameManagerClient : IGameManager
    {

        private readonly Uri _endpointAddress = new Uri("net.tcp://localhost:8555/EntityFX.EconomicsArcade.Manager/EntityFX.EconomicsArcade.Contract.Manager.GameManager.IGameManager");
        private Guid _sesionGuid;

        public void AddSession()///Delete this
        {

        }

        public GameManagerClient(Guid sesionGuid)
        {
            _sesionGuid = sesionGuid;
        }

        public void BuyFundDriver(int fundDriverId)
        {
            using (var proxy = new GameManagerProxy(_sesionGuid))
            {
                var channel = proxy.CreateChannel(_endpointAddress);
                proxy.ApplyContextScope();
                channel.BuyFundDriver(fundDriverId);
                proxy.CloseChannel();
            }
        }

        public void PerformManualStep()
        {
            using (var proxy = new GameManagerProxy(_sesionGuid))
            {
                var channel = proxy.CreateChannel(_endpointAddress);
                proxy.ApplyContextScope();
                channel.PerformManualStep();
                proxy.CloseChannel();
            }
        }

        public void FightAgainstInflation()
        {
            throw new System.NotImplementedException();
        }

        public void PlayLottery()
        {
            throw new System.NotImplementedException();
        }

        public FundsCounters GetCounters()
        {
            FundsCounters counters;
            using (var proxy = new GameManagerProxy(_sesionGuid))
            {
                var channel = proxy.CreateChannel(_endpointAddress);
                proxy.ApplyContextScope();
                counters = channel.GetCounters();
                proxy.CloseChannel();
            }
            return counters;
        }

        public GameData GetGameData()
        {
            GameData gameData;
            using (var proxy = new GameManagerProxy(_sesionGuid))
            {
                var channel = proxy.CreateChannel(_endpointAddress);
                proxy.ApplyContextScope();
                gameData = channel.GetGameData();
                proxy.CloseChannel();
            }
            return gameData;
        }
    }
}
using System;
using EntityFX.EconomicsArcade.Contract.Common;
using EntityFX.EconomicsArcade.Contract.Common.Counters;
using EntityFX.EconomicsArcade.Contract.Manager.GameManager;
using EntityFX.EconomicsArcade.Infrastructure.Service;

namespace EntityFX.EconomicsArcade.Utils.ClientProxy.Manager
{
    public class GameManagerProxy : IGameManager
    {
        private readonly InfrastructureProxyFactory<IGameManager> _proxyFactory;
        private readonly Uri _endpointAddress = new Uri("net.tcp://localhost:8555/EntityFX.EconomicsArcade.Manager/EntityFX.EconomicsArcade.Contract.Manager.GameManager.IGameManager");

        public GameManagerProxy(InfrastructureProxyFactory<IGameManager> proxyFactory)
        {
            _proxyFactory = proxyFactory;
        }

        public void BuyFundDriver(int fundDriverId)
        {
            var proxy = _proxyFactory.OpenChannel(_endpointAddress);
            proxy.BuyFundDriver(fundDriverId);
            _proxyFactory.CloseChannel();
        }

        public void PerformManualStep()
        {
            var proxy = _proxyFactory.OpenChannel(_endpointAddress);
            proxy.PerformManualStep();
            _proxyFactory.CloseChannel();
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
            var proxy = _proxyFactory.OpenChannel(_endpointAddress);
            var results = proxy.GetCounters();
            _proxyFactory.CloseChannel();
            return results;
        }

        public GameData GetGameData()
        {
            var proxy = _proxyFactory.OpenChannel(_endpointAddress);
            var results = proxy.GetGameData();
            _proxyFactory.CloseChannel();
            return results;
        }
    }
}
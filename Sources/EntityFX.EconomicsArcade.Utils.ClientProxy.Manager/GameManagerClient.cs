using System;
using EntityFX.EconomicsArcade.Contract.Common;
using EntityFX.EconomicsArcade.Contract.Common.Counters;
using EntityFX.EconomicsArcade.Contract.Manager.GameManager;
using EntityFX.EconomicsArcade.Infrastructure.Common;
using EntityFX.EconomicsArcade.Infrastructure.Service;

namespace EntityFX.EconomicsArcade.Utils.ClientProxy.Manager
{
    public class GameManagerClient<TInfrastructureProxy> : IGameManager
                                where TInfrastructureProxy : InfrastructureProxy<IGameManager>, new()
    {

        private readonly Uri _endpointAddress;// = new Uri("net.tcp://localhost:8555/EntityFX.EconomicsArcade.Manager/EntityFX.EconomicsArcade.Contract.Manager.GameManager.IGameManager");
        private Guid _sesionGuid;
        private ILogger _logger;

        private static readonly Action<Guid> operationContext = (_) =>
        {
            OperationContextHelper.Instance.SessionId = _;
        };

        public GameManagerClient(ILogger logger, string endpointAddress, Guid sesionGuid)
        {
            _logger = logger;
            _sesionGuid = sesionGuid;
            _endpointAddress = new Uri(endpointAddress);
        }

        public BuyFundDriverResult BuyFundDriver(int fundDriverId)
        {
            using (var proxy = new TInfrastructureProxy())
            {
                var channel = proxy.CreateChannel(_endpointAddress);
                proxy.ApplyContextScope(operationContext, _sesionGuid);
                var result = channel.BuyFundDriver(fundDriverId);
                proxy.CloseChannel();
                return result;
            }
        }

        public ManualStepResult PerformManualStep(VerificationManualStepResult verificationManualStepResult)
        {
            using (var proxy = new TInfrastructureProxy())
            {
                var channel = proxy.CreateChannel(_endpointAddress);
                proxy.ApplyContextScope(operationContext, _sesionGuid);
                var res = channel.PerformManualStep(verificationManualStepResult);
                proxy.CloseChannel();
                return res;
            }
        }

        public void FightAgainstInflation()
        {
            using (var proxy = new TInfrastructureProxy())
            {
                var channel = proxy.CreateChannel(_endpointAddress);
                proxy.ApplyContextScope(operationContext, _sesionGuid);
                channel.FightAgainstInflation();
                proxy.CloseChannel();
            }
        }

        public void PlayLottery()
        {
            throw new System.NotImplementedException();
        }

        public FundsCounters GetCounters()
        {
            FundsCounters counters;
            using (var proxy = new TInfrastructureProxy())
            {
                var channel = proxy.CreateChannel(_endpointAddress);
                proxy.ApplyContextScope(operationContext, _sesionGuid);
                counters = channel.GetCounters();
                proxy.CloseChannel();
            }
            return counters;
        }

        public GameData GetGameData()
        {
            GameData gameData = null;
            using (var proxy = new TInfrastructureProxy())
            {
                var channel = proxy.CreateChannel(_endpointAddress);
                proxy.ApplyContextScope(operationContext, _sesionGuid);
                gameData = channel.GetGameData();
                proxy.CloseChannel();
            }
            return gameData;
        }

        public void ActivateDelayedCounter(int counterId)
        {
            using (var proxy = new TInfrastructureProxy())
            {
                var channel = proxy.CreateChannel(_endpointAddress);
                proxy.ApplyContextScope(operationContext, _sesionGuid);
                channel.ActivateDelayedCounter(counterId);
                proxy.CloseChannel();
            }
        }
    }
}
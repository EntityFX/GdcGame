using System;
using System.ServiceModel.Channels;
using EntityFX.Gdcame.Common.Contract;
using EntityFX.Gdcame.Common.Contract.Counters;
using EntityFX.Gdcame.Infrastructure.Common;
using EntityFX.Gdcame.Infrastructure.Service.Bases;
using EntityFX.Gdcame.Manager.Contract.GameManager;

namespace EntityFX.Gdcame.Utils.ClientProxy.WcfManager
{
    public class GameManagerClient<TInfrastructureProxy> : IGameManager
        where TInfrastructureProxy : IInfrastructureProxy<IGameManager, Binding>, new()
    {
        private readonly Uri _endpointAddress;
        // = new Uri("net.tcp://localhost:8555/EntityFX.EconomicsArcade.Manager/EntityFX.EconomicsArcade.Contract.Manager.GameManager.IGameManager");

        private readonly IOperationContextHelper _operationContextHelper;
        private readonly Guid _sesionGuid;


        private readonly Action<Guid> operationContext;
        private ILogger _logger;

        public GameManagerClient(ILogger logger, IOperationContextHelper operationContextHelper, string endpointAddress,
            Guid sesionGuid)
        {
            _logger = logger;
            _operationContextHelper = operationContextHelper;
            _sesionGuid = sesionGuid;
            _endpointAddress = new Uri(endpointAddress);
            operationContext = _ => _operationContextHelper.Instance.SessionId = _;
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
            throw new NotImplementedException();
        }

        public Cash GetCounters()
        {
            Cash cash;
            using (var proxy = new TInfrastructureProxy())
            {
                var channel = proxy.CreateChannel(_endpointAddress);
                proxy.ApplyContextScope(operationContext, _sesionGuid);
                cash = channel.GetCounters();
                proxy.CloseChannel();
            }
            return cash;
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

        public bool Ping()
        {
            var result = false;
            using (var proxy = new TInfrastructureProxy())
            {
                var channel = proxy.CreateChannel(_endpointAddress);
                proxy.ApplyContextScope(operationContext, _sesionGuid);
                channel.Ping();
                proxy.CloseChannel();
            }
            return result;
        }
    }
}
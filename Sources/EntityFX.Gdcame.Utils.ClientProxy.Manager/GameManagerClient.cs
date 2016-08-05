using System;
using EntityFX.Gdcame.Common.Contract;
using EntityFX.Gdcame.Common.Contract.Counters;
using EntityFX.Gdcame.Infrastructure.Common;
using EntityFX.Gdcame.Infrastructure.Service.Bases;
using EntityFX.Gdcame.Manager.Contract.GameManager;

namespace EntityFX.Gdcame.Utils.ClientProxy.Manager
{
    public class GameManagerClient<TInfrastructureProxy> : IGameManager
        where TInfrastructureProxy : InfrastructureProxy<IGameManager>, new()
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
                IGameManager channel = proxy.CreateChannel(_endpointAddress);
                proxy.ApplyContextScope(operationContext, _sesionGuid);
                BuyFundDriverResult result = channel.BuyFundDriver(fundDriverId);
                proxy.CloseChannel();
                return result;
            }
        }

        public ManualStepResult PerformManualStep(VerificationManualStepResult verificationManualStepResult)
        {
            using (var proxy = new TInfrastructureProxy())
            {
                IGameManager channel = proxy.CreateChannel(_endpointAddress);
                proxy.ApplyContextScope(operationContext, _sesionGuid);
                ManualStepResult res = channel.PerformManualStep(verificationManualStepResult);
                proxy.CloseChannel();
                return res;
            }
        }

        public void FightAgainstInflation()
        {
            using (var proxy = new TInfrastructureProxy())
            {
                IGameManager channel = proxy.CreateChannel(_endpointAddress);
                proxy.ApplyContextScope(operationContext, _sesionGuid);
                channel.FightAgainstInflation();
                proxy.CloseChannel();
            }
        }

        public void PlayLottery()
        {
            throw new NotImplementedException();
        }

        public FundsCounters GetCounters()
        {
            FundsCounters counters;
            using (var proxy = new TInfrastructureProxy())
            {
                IGameManager channel = proxy.CreateChannel(_endpointAddress);
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
                IGameManager channel = proxy.CreateChannel(_endpointAddress);
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
                IGameManager channel = proxy.CreateChannel(_endpointAddress);
                proxy.ApplyContextScope(operationContext, _sesionGuid);
                channel.ActivateDelayedCounter(counterId);
                proxy.CloseChannel();
            }
        }

        public bool Ping()
        {
            bool result = false;
            using (var proxy = new TInfrastructureProxy())
            {
                IGameManager channel = proxy.CreateChannel(_endpointAddress);
                proxy.ApplyContextScope(operationContext, _sesionGuid);
                channel.Ping();
                proxy.CloseChannel();
            }
            return result;
        }
    }
}
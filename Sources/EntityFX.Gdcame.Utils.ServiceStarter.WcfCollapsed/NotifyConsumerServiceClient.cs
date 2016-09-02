using System;
using System.ServiceModel.Channels;
using EntityFX.Gdcame.Common.Contract;
using EntityFX.Gdcame.Infrastructure.Service.Bases;
using EntityFX.Gdcame.NotifyConsumer.Contract;

namespace EntityFX.Gdcame.Utils.ServiceStarter.WcfCollapsed
{
    internal class NotifyConsumerServiceClient<TInfrastructureProxy> : INotifyConsumerService
        where TInfrastructureProxy : IInfrastructureProxy<INotifyConsumerService, Binding>, new()
    {
        public void PushGameData(UserContext userContext, GameData gameData)
        {
            throw new NotImplementedException();
        }
    }
}

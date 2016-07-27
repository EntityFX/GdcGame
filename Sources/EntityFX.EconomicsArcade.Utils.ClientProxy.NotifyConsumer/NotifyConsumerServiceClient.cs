using System;
using EntityFX.EconomicsArcade.Contract.Common;
using EntityFX.EconomicsArcade.Contract.NotifyConsumerService;
using EntityFX.EconomicsArcade.Infrastructure.Service;

namespace EntityFX.EconomicsArcade.Utils.ClientProxy.NotifyConsumer
{
    public class NotifyConsumerServiceClient<TInfrastructureProxy> : INotifyConsumerService
                                        where TInfrastructureProxy : InfrastructureProxy<INotifyConsumerService>, new()
    {
        private readonly Uri _endpoint;

        public NotifyConsumerServiceClient(string endpoint)
        {
            _endpoint = new Uri(endpoint);
        }

        public void PushGameData(UserContext userContext, GameData gameData)
        {
            using (var proxy = new TInfrastructureProxy())
            {
                var channel = proxy.CreateChannel(_endpoint);
                channel.PushGameData(userContext, gameData);
                proxy.CloseChannel();
            }
        }
    }
}

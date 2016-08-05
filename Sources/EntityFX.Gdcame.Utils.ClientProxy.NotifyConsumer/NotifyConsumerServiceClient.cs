using System;
using EntityFX.Gdcame.Common.Contract;
using EntityFX.Gdcame.Infrastructure.Service.Bases;
using EntityFX.Gdcame.NotifyConsumer.Contract;

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

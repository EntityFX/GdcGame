﻿using System;
using EntityFX.EconomicsArcade.Contract.Common;
using EntityFX.EconomicsArcade.Contract.NotifyConsumerService;

namespace EntityFX.EconomicsArcade.Utils.ClientProxy.NotifyConsumer
{
    public class NotifyConsumerServiceClient : INotifyConsumerService
    {
        private readonly Uri _endpoint;

        public NotifyConsumerServiceClient(string endpoint)
        {
            _endpoint = new Uri(endpoint);
        }

        public void PushGameData(int userId, GameData gameData)
        {
            using (var proxy = new NotifyConsumerServiceProxy())
            {
                var channel = proxy.CreateChannel(_endpoint);
                channel.PushGameData(userId, gameData);
                proxy.CloseChannel();
            }
        }
    }
}

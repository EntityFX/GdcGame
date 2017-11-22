﻿using EntityFX.Gdcame.Infrastructure.Common;
using EntityFX.Gdcame.NotifyConsumer.Contract;

namespace EntityFX.Gdcame.Utils.MainServer
{
    using EntityFX.Gdcame.Engine.GameEngine.NetworkGameEngine;

    public class NotifyConsumerClientFactory : INotifyConsumerClientFactory
    {
        private readonly IResolver _unityContainer;

        public NotifyConsumerClientFactory(IResolver unityContainer)
        {
            _unityContainer = unityContainer;
        }


        public INotifyConsumerService BuildNotifyConsumerClient(string name = null)
        {
            var game = string.IsNullOrEmpty(name)
                ? _unityContainer.Resolve<INotifyConsumerService>()
                : _unityContainer.Resolve<INotifyConsumerService>(name);
            return game;
        }
    }
}
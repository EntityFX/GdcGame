using System;
using EntityFX.Gdcame.GameEngine.NetworkGameEngine;
using EntityFX.Gdcame.NotifyConsumer.Contract;
using Microsoft.Practices.Unity;

namespace EntityFX.Gdcame.Utils.Common
{
    public class NotifyConsumerClientFactory : INotifyConsumerClientFactory
    {
        private readonly IUnityContainer _unityContainer;
        private readonly string _name;

        public NotifyConsumerClientFactory(IUnityContainer unityContainer, string name)
        {
            _unityContainer = unityContainer;
            _name = name;
        }


        public INotifyConsumerService BuildNotifyConsumerClient()
        {
            
            var game = String.IsNullOrEmpty(_name) ? _unityContainer.Resolve<INotifyConsumerService>() : _unityContainer.Resolve<INotifyConsumerService>(_name);
            return game;
        }
    }
}
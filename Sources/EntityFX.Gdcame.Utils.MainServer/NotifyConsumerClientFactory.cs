using EntityFX.Gdcame.NotifyConsumer.Contract;
using Microsoft.Practices.Unity;

namespace EntityFX.Gdcame.Utils.MainServer
{
    using EntityFX.Gdcame.Engine.GameEngine.NetworkGameEngine;

    public class NotifyConsumerClientFactory : INotifyConsumerClientFactory
    {
        private readonly string _name;
        private readonly IUnityContainer _unityContainer;

        public NotifyConsumerClientFactory(IUnityContainer unityContainer, string name)
        {
            _unityContainer = unityContainer;
            _name = name;
        }


        public INotifyConsumerService BuildNotifyConsumerClient()
        {
            var game = string.IsNullOrEmpty(_name)
                ? _unityContainer.Resolve<INotifyConsumerService>()
                : _unityContainer.Resolve<INotifyConsumerService>(_name);
            return game;
        }
    }
}
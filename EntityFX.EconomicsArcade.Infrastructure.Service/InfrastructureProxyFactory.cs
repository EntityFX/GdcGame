using System;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace EntityFX.EconomicsArcade.Infrastructure.Service
{
    public abstract class InfrastructureProxyFactory<TProxy> : IDisposable
    {
        private TProxy _clientProxy;
        private ChannelFactory<TProxy> _channelFactory;
        private OperationContextScope _scope;

        protected virtual TProxy CreateClientProxy(Uri remoteAddress)
        {
            var binding = GetBinding();
            _channelFactory = new ChannelFactory<TProxy>(binding, new EndpointAddress(remoteAddress));
            return _channelFactory.CreateChannel();
        }

        protected virtual OperationContextScope CreateOperationContextScope()
        {
            var operationContext = new OperationContextScope((IContextChannel)_clientProxy);
            ApplyOperationContext();
            return operationContext;
        }

        protected abstract Binding GetBinding();
        protected abstract void ApplyOperationContext();

        public TProxy Create(Uri remoteAddress)
        {
            _clientProxy = CreateClientProxy(remoteAddress);
            _scope = CreateOperationContextScope();
            return _clientProxy;
        }

        public void Close()
        {
            if (_channelFactory != null) _channelFactory.Close();
            if (_clientProxy != null) ((IClientChannel)_clientProxy).Close();
        }
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (_scope != null) _scope.Dispose();
            if (_clientProxy != null) ((IClientChannel)_clientProxy).Dispose();
            if (_channelFactory != null) ((IDisposable)_channelFactory).Dispose();
        }
    }
}
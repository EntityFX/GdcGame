using System;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace EntityFX.EconomicsArcade.Infrastructure.Service
{
    public abstract class InfrastructureProxyFactory<TServiceContract> : IDisposable
    {
        private TServiceContract _clientProxy;
        private OperationContextScope _operationContextScope;
        private ChannelFactory<TServiceContract> _channelFactory;

        protected virtual TServiceContract CreateProxy(Uri endpointAddress)
        {
            var binding = GetBinding();
            _channelFactory = new ChannelFactory<TServiceContract>(binding, new EndpointAddress(endpointAddress));
            return _channelFactory.CreateChannel();
        }

        protected abstract Binding GetBinding();

        private OperationContextScope CreateContextScope()
        {
            return new OperationContextScope((IContextChannel)_clientProxy);
        }

        protected virtual void ApplyOperationContext()
        {

        }

        public TServiceContract OpenChannel(Uri endpointAddress)
        {
            _clientProxy = CreateProxy(endpointAddress);
            _operationContextScope = CreateContextScope();
            ApplyOperationContext();
            return _clientProxy;
        }

        public void CloseChannel()
        {
            if (_clientProxy != null) ((IClientChannel)_clientProxy).Close();
            if (_channelFactory != null) _channelFactory.Close();
        }

        public void Dispose()
        {
            if (_clientProxy != null) _operationContextScope.Dispose();
            if (_clientProxy != null) ((IDisposable)_clientProxy).Dispose();
            if (_channelFactory != null) ((IDisposable)_channelFactory).Dispose();
        }
    }
}

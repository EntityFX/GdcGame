using System;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace EntityFX.Gdcame.Infrastructure.Service.Bases
{

	public interface IInfrastructureProxy<TServiceContract, out TBinding> : IDisposable
		where TBinding : Binding 
	{
		TServiceContract CreateChannel (Uri endpointAddress);
		TServiceContract ApplyContextScope ();
		TServiceContract ApplyContextScope<TContextType> (Action<TContextType> applyContextAction, TContextType contextData);
		void CloseChannel ();
	}

	public abstract class InfrastructureProxy<TServiceContract, TBinding> : IDisposable,
		IInfrastructureProxy<TServiceContract, TBinding>
		where TBinding : Binding
    {
        private TServiceContract _clientProxy;
        private OperationContextScope _operationContextScope;
        private ChannelFactory<TServiceContract> _channelFactory;

        public virtual TServiceContract CreateChannel(Uri endpointAddress)
        {
			var binding = GetBindingFactory ().Build (null);
            _channelFactory = new ChannelFactory<TServiceContract>(binding, new EndpointAddress(endpointAddress));
            _clientProxy = _channelFactory.CreateChannel();
            return _clientProxy;
        }

		protected abstract IBindingFactory<TBinding> GetBindingFactory ();

        private OperationContextScope CreateContextScope()
        {
            return new OperationContextScope((IContextChannel)_clientProxy);
        }

        protected virtual void ApplyOperationContext(dynamic contextData)
        {

        }

        public TServiceContract ApplyContextScope()
        {
            return ApplyContextScope<dynamic>(ApplyOperationContext, null);
        }

		public virtual TServiceContract ApplyContextScope<TContextType>(Action<TContextType> applyContextAction, TContextType contextData) 
        {
            if (_clientProxy == null)
            {
                throw new InvalidOperationException("Channel proxy is not created");
            }
            _operationContextScope = CreateContextScope();
            applyContextAction(contextData);
            return _clientProxy;
        }

		public void CloseChannel()
        {
            if (_clientProxy != null) ((IClientChannel)_clientProxy).Close();
            if (_channelFactory != null) _channelFactory.Close();
        }

        public void Dispose()
        {
            if (_operationContextScope != null) _operationContextScope.Dispose();
			if (_clientProxy != null) ((IDisposable)_clientProxy).Dispose();
			/*try {
				if (_channelFactory != null) {
					if (_channelFactory.State != CommunicationState.Faulted) {
						_channelFactory.Close();
					} else {
						_channelFactory.Abort();
					}
				}
			}
			catch(CommunicationException) {
				_channelFactory.Abort();
			}
			catch(TimeoutException) {
				_channelFactory.Abort();
			}
			catch(Exception) {
				_channelFactory.Abort();
				throw;
			}
			finally {
				_channelFactory = null;

			}*/
        }
    }
}

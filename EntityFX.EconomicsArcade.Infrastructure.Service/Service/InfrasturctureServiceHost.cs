using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace EntityFX.EconomicsArcade.Infrastructure.Service.Service
{
    public abstract class InfrasturctureServiceHost<T> : IDisposable
    {
        private ServiceHost _serviceHost; 
        
        protected virtual Binding GetBinding()
        {
            return new NetTcpBinding();
        }

        protected virtual ServiceEndpoint CreateServiceEndpoint()
        {

        }
    }
}

using System;
using System.ServiceModel;

namespace EntityFX.EconomicsArcade.Infrastructure.Service
{
    public interface IServiceHost
    {
        string Name
        {
            get;
        }

        Uri Endpoint
        {
            get;
        }

        ServiceHost ServiceHost { get; }

        void Open(Uri endpointAddress);

        void Close();
    }
}

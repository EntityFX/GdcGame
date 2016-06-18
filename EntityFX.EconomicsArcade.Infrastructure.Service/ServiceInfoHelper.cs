using System;
using System.Linq;
using System.ServiceModel;

namespace EntityFX.EconomicsArcade.Infrastructure.Service
{
    public static class ServiceInfoHelper
    {
        public static void PrintServiceHostInfo(ServiceHost serviceHost)
        {
            Console.WriteLine("Service {0}", serviceHost.Description.Name);
            serviceHost.Description.Endpoints.ToList().ForEach(_ =>
            {
                Console.WriteLine("Endpoint");
                Console.WriteLine("\tBinding: {0}", _.Binding.GetType());
                Console.WriteLine("\tUri: {0}", _.ListenUri);
                Console.WriteLine("\tContract: {0}", _.Contract.ContractType.FullName);
            });
        }
    }
}
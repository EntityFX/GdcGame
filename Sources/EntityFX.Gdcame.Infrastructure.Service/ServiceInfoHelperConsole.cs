using System;
using System.Linq;
using System.ServiceModel;
using EntityFX.Gdcame.Infrastructure.Service.Interfaces;

namespace EntityFX.Gdcame.Infrastructure.Service
{
    public class ServiceInfoHelperConsole : IServiceInfoHelper
    {
        public void PrintServiceHostInfo(ServiceHost serviceHost)
        {
            Console.WriteLine("Service {0}", serviceHost.Description.Name);
            serviceHost.Description.Endpoints.ToList().ForEach(_ =>
            {
                Console.WriteLine("Endpoint");
                Console.WriteLine("\tBinding: {0}", _.Binding.GetType());
                Console.WriteLine("\tUri: {0}", _.ListenUri);
                Console.WriteLine("\tContract: {0}", _.Contract.ContractType.FullName);
                Console.WriteLine("{0}", new String('-', Console.WindowWidth));
            });
        }
    }
}
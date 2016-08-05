using System.Linq;
using System.ServiceModel;
using EntityFX.Gdcame.Infrastructure.Common;
using EntityFX.Gdcame.Infrastructure.Service.Interfaces;

namespace EntityFX.Gdcame.Infrastructure.Service
{
    public class ServiceInfoHelperLogger : IServiceInfoHelper
    {
        private ILogger _logger;

        public ServiceInfoHelperLogger(ILogger logger)
        {
            _logger = logger;
        }

        public void PrintServiceHostInfo(ServiceHost serviceHost)
        {
            _logger.Info("Service {0}", serviceHost.Description.Name);
            serviceHost.Description.Endpoints.ToList().ForEach(_ =>
            {
                _logger.Info("Endpoint");
                _logger.Info("\tBinding: {0}", _.Binding.GetType());
                _logger.Info("\tUri: {0}", _.ListenUri);
                _logger.Info("\tContract: {0}", _.Contract.ContractType.FullName);
            });
        }
    }
}

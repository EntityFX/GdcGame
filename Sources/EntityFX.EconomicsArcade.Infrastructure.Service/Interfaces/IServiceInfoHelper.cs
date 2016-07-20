using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ServiceModel;

namespace EntityFX.EconomicsArcade.Infrastructure.Service.Interfaces
{
    public interface IServiceInfoHelper
    {
        void PrintServiceHostInfo(ServiceHost serviceHost);
    }
}

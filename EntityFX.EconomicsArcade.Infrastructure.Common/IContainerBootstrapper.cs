using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFX.EconomicsArcade.Infrastructure.Common
{
    public interface IContainerBootstrapper
    {
        IUnityContainer Configure(IUnityContainer container);
    }
}

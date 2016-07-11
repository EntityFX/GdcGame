using Microsoft.Practices.Unity;

namespace EntityFX.EconomicsArcade.Infrastructure.Common
{
    public interface IContainerBootstrapper
    {
        IUnityContainer Configure(IUnityContainer container);
    }
}

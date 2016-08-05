using Microsoft.Practices.Unity;

namespace EntityFX.Gdcame.Infrastructure.Common
{
    public interface IContainerBootstrapper
    {
        IUnityContainer Configure(IUnityContainer container);
    }
}

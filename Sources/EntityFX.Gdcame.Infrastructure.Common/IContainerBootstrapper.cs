
namespace EntityFX.Gdcame.Infrastructure.Common
{
    public interface IContainerBootstrapper
    {
        IIocContainer Configure(IIocContainer container);
    }
}
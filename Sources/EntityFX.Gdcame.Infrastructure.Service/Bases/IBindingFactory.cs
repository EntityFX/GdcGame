using System.ServiceModel.Channels;

namespace EntityFX.Gdcame.Infrastructure.Service.Bases
{
    public interface IBindingFactory<TBinding>
        where TBinding : Binding
    {
        TBinding Build(object config);
    }
}
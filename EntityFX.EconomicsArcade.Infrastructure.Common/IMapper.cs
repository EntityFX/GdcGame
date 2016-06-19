namespace EntityFX.EconomicsArcade.Infrastructure.Common
{
    public interface IMapper<in TSource, out TDestination>
    {
        TDestination Map(TSource source);
    }
}
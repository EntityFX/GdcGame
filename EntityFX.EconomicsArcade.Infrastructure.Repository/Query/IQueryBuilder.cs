namespace EntityFX.EconomicsArcade.Infrastructure.Repository.Query
{
    public interface IQueryBuilder
    {
        IQueryFor<TResult> For<TResult>();
    }
}

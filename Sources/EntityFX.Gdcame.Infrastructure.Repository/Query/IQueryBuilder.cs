namespace EntityFX.Gdcame.Infrastructure.Repository.Query
{
    public interface IQueryBuilder
    {
        IQueryFor<TResult> For<TResult>();
    }
}

using EntityFX.Gdcame.Infrastructure.Repository.Criterion;

namespace EntityFX.Gdcame.Infrastructure.Repository.Query
{
    public interface IQueryFor<out T>
    {
        T With<TCriterion>(TCriterion criterion) where TCriterion : ICriterion;
    }
}

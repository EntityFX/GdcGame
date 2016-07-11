using EntityFX.EconomicsArcade.Infrastructure.Repository.Criterion;

namespace EntityFX.EconomicsArcade.Infrastructure.Repository.Query
{
    public interface IQueryFor<out T>
    {
        T With<TCriterion>(TCriterion criterion) where TCriterion : ICriterion;
    }
}

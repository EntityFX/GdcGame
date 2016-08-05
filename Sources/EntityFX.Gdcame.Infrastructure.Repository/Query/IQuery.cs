using EntityFX.Gdcame.Infrastructure.Repository.Criterion;

namespace EntityFX.Gdcame.Infrastructure.Repository.Query
{
    public interface IQuery<in TCriterion, out TResult>
        where TCriterion : ICriterion
    {
        TResult Execute(TCriterion criterion);
    }
}

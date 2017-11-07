using EntityFX.Gdcame.Infrastructure.Common;
using EntityFX.Gdcame.Infrastructure.Repository.Criterion;

namespace EntityFX.Gdcame.Infrastructure.Repository.Query
{
    public class QueryFor<TResult> : IQueryFor<TResult>
    {
        private readonly IIocContainer _scope;

        public QueryFor(IIocContainer scope)
        {
            _scope = scope;
        }

        #region Implementation of IQueryFor<out TResult>

        public TResult With<TCriterion>(TCriterion criterion) where TCriterion : ICriterion
        {
            return _scope.Resolve<IQuery<TCriterion, TResult>>().Execute(criterion);
        }

        #endregion
    }
}
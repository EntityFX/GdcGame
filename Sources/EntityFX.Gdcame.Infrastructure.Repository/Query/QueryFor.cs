using EntityFX.Gdcame.Infrastructure.Repository.Criterion;
using Microsoft.Practices.Unity;

namespace EntityFX.Gdcame.Infrastructure.Repository.Query
{
    public class QueryFor<TResult> : IQueryFor<TResult>
    {
        private readonly IUnityContainer _scope;

        public QueryFor(IUnityContainer scope)
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

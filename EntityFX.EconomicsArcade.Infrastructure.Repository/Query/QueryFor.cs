using EntityFX.EconomicsArcade.Infrastructure.Repository.Criterion;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFX.EconomicsArcade.Infrastructure.Repository.Query
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

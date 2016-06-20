using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFX.EconomicsArcade.Infrastructure.Repository.Query
{
    public class QueryBuilder : IQueryBuilder
    {
        private readonly IUnityContainer _scope;

        public QueryBuilder(IUnityContainer scope)
        {
            _scope = scope;
        }

        #region Implementation of IQueryBuilder

        public IQueryFor<TResult> For<TResult>()
        {
            return new QueryFor<TResult>(_scope);
        }

        #endregion
    }
}

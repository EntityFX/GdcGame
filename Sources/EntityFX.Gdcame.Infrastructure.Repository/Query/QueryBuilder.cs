using EntityFX.Gdcame.Infrastructure.Common;

namespace EntityFX.Gdcame.Infrastructure.Repository.Query
{
    public class QueryBuilder : IQueryBuilder
    {
        private readonly IIocContainer _scope;

        public QueryBuilder(IIocContainer scope)
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
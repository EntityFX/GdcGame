using System.Data.Entity;

namespace EntityFX.Gdcame.Infrastructure.Repository.Query
{
    public abstract class QueryBase
    {
        private readonly DbContext _dbContext;

        protected DbContext DbContext
        {
            get
            {
                return _dbContext;
            }
        }

        protected QueryBase(DbContext dbContext)
        {
            _dbContext = dbContext;
        }
    }
}

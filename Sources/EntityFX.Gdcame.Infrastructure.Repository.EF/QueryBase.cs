using Microsoft.EntityFrameworkCore;

namespace EntityFX.Gdcame.Infrastructure.Repository.EF
{
    public abstract class QueryBase
    {
        protected QueryBase(DbContext dbContext)
        {
            DbContext = dbContext;
        }

        protected DbContext DbContext { get; private set; }
    }
}
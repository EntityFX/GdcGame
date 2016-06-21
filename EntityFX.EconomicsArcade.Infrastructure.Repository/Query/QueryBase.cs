using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFX.EconomicsArcade.Infrastructure.Repository.Query
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

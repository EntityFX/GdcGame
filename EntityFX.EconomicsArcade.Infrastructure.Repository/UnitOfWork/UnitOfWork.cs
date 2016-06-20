using EntityFX.EconomicsArcade.Infrastructure.Repository.Query;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFX.EconomicsArcade.Infrastructure.Repository.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext _dbContext;
        private readonly IQueryBuilder _queryBuilder;

        public UnitOfWork(DbContext dbContext, IQueryBuilder queryBuilder)
        {
            _dbContext = dbContext;
            _queryBuilder = queryBuilder;
        }
        
        public void Commit()
        {
            _dbContext.SaveChanges();
        }

        public TEntity CreateEntity<TEntity>() where TEntity : class, new()
        {
            var enity = new TEntity();
            _dbContext.Set<TEntity>().Add(enity);
            return enity;
        }

        public void DeleteEntity<TEntity>(TEntity entity) where TEntity : class
        {
            _dbContext.Set<TEntity>().Remove(entity);
        }

        public Query.IQueryBuilder BuildQuery()
        {
            return _queryBuilder;
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}

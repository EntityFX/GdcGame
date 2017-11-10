﻿using System;

using System.Linq.Expressions;
using EntityFX.Gdcame.Infrastructure.Repository.Query;
using EntityFX.Gdcame.Infrastructure.Repository.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace EntityFX.Gdcame.Infrastructure.Repository.EF
{
    public class EfUnitOfWork : IUnitOfWork
    {
        private readonly DbContext _dbContext;
        private readonly IQueryBuilder _queryBuilder;

        public EfUnitOfWork(DbContext dbContext, IQueryBuilder queryBuilder)
        {
            _dbContext = dbContext;
            _queryBuilder = queryBuilder;
        }

        public void Commit()
        {
            _dbContext.SaveChanges();
        }

        public void ExcludeProperty<TEntity, TProperty>(TEntity entity, Expression<Func<TEntity, TProperty>> property)
            where TEntity : class
        {
            var entry = _dbContext.Entry(entity);
            entry.State = EntityState.Modified;
            entry.Property(property).IsModified = false;
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

        public TEntity AttachEntity<TEntity>(TEntity entity) where TEntity : class
        {
            _dbContext.Set<TEntity>().Attach(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;
            return entity;
        }

        public IQueryBuilder BuildQuery()
        {
            return _queryBuilder;
        }

        public void Dispose()
        {
            Dispose(true);
           // GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposeManaged)
        {
            if (disposeManaged)
            {
                _dbContext.Dispose();
            }
        }
    }
}
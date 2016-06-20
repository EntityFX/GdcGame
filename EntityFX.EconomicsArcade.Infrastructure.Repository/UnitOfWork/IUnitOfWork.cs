﻿using EntityFX.EconomicsArcade.Infrastructure.Repository.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFX.EconomicsArcade.Infrastructure.Repository.UnitOfWork
{
    /// <summary>
    /// An abstraction for unit of work pattern.
    /// </summary>
    /// <remarks>
    /// The main goal of unit of work is to separate one set of changes from another. Eack unit of work represents a single transaction.
    /// </remarks>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Commits any changed made during unit of work scope.
        /// </summary>
        void Commit();

        /// <summary>
        /// Creates a new entity instance of type <see cref="TEntity"/>. A new instance would participate inside unit of work.
        /// </summary>
        /// <typeparam name="TEntity">Returns a new instance of type <see cref="TEntity"/>. </typeparam>
        /// <returns></returns>
        TEntity CreateEntity<TEntity>() where TEntity : class, new();

        /// <summary>
        /// Deletes an entity instance of type <see cref="TEntity"/>.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        void DeleteEntity<TEntity>(TEntity entity) where TEntity : class;

        IQueryBuilder BuildQuery();
    }
}

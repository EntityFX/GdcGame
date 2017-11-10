using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFX.Gdcame.DataAccess.Repository.Ef.Common
{
    using System.ComponentModel.DataAnnotations.Schema;
    using Microsoft.EntityFrameworkCore;

    using EntityFX.Gdcame.DataAccess.Repository.Ef.Common.Entities;

    public class DbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public virtual DbSet<UserEntity> Users { get; set; }

        public DbContext()
            : base()
        {
        }

        public DbContext(string connectionString)
    : base()
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserEntity>().ToTable("User").HasKey(_ => _.Id);

            modelBuilder.Entity<UserEntity>().Property(_ => _.Email).IsRequired();

            modelBuilder.Entity<UserEntity>()
                .HasKey(e => e.Id)
                .Property(e => e.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

        }
    }
}

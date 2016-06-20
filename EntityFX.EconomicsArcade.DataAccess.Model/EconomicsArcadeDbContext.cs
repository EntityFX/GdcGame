using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFX.EconomicsArcade.DataAccess.Model
{
    public class EconomicsArcadeDbContext : DbContext
    {
        public DbSet<UserEntity> UserEntitySet { get; set; }

        public EconomicsArcadeDbContext()
            :base("EconomicsArcadeDbContext")
        {

        }

        public EconomicsArcadeDbContext(string connectionString)
            : base(connectionString)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<IncrementorEntity>()
                .HasRequired<FundsDriverEntity>(_ => _.FundsDriver)
                .WithMany(_ => _.Incrementors)
                .HasForeignKey(_ => _.FundsDriverId);
        }
    }
}

using System.Data.Entity;

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
                .HasRequired(_ => _.FundsDriver)
                .WithMany(_ => _.Incrementors)
                .HasForeignKey(_ => _.FundsDriverId);

            modelBuilder.Entity<IncrementorEntity>()
                .HasRequired(_ => _.Counter)
                .WithMany(_ => _.Incrementors)
                .HasForeignKey(_ => _.CounterId);
        }
    }
}

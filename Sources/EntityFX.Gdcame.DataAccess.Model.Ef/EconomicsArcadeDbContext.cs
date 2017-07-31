using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace EntityFX.Gdcame.DataAccess.Model.Ef
{
    public class EconomicsArcadeDbContext : DbContext
    {
        public EconomicsArcadeDbContext()
            : base("EconomicsArcadeDbContext")
        {
        }

        public EconomicsArcadeDbContext(string connectionString)
            : base(connectionString)
        {
        }

        public virtual DbSet<CounterEntity> Counters { get; set; }
        public virtual DbSet<FundsDriverEntity> FundsDrivers { get; set; }
        public virtual DbSet<IncrementorEntity> Incrementors { get; set; }


        public virtual DbSet<UserGameDataSnapshotEntity> UserGameDataSnapshot { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CounterEntity>()
                .ToTable("Counter")
                .HasKey(_ => _.Id);


            modelBuilder.Entity<CounterEntity>()
                .Property(e => e.InitialValue)
                .HasColumnType("Money");

            modelBuilder.Entity<FundsDriverEntity>()
                .ToTable("FundsDriver")
                .HasKey(_ => _.Id);

            modelBuilder.Entity<FundsDriverEntity>()
                .HasMany(e => e.Incrementors)
                .WithRequired(e => e.FundsDriver)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<FundsDriverEntity>()
                .Property(e => e.InitialValue)
                .HasColumnType("Money");

            modelBuilder.Entity<FundsDriverEntity>()
                .Property(e => e.UnlockValue)
                .HasColumnType("Money");

            modelBuilder.Entity<IncrementorEntity>()
                .ToTable("Incrementor")
                .HasKey(_ => _.Id);

            modelBuilder.Entity<IncrementorEntity>()
                .Property(e => e.Value)
                .HasColumnType("Money");


            /*modelBuilder.Entity<UserEntity>()
                 .HasMany(e => e.UserGameDataSnapshotEntity)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);      */


            modelBuilder.Entity<CustomRuleEntity>()
                .ToTable("CustomRule");

            modelBuilder.Entity<CustomRuleEntity>()
                .HasMany(e => e.FundsDrivers)
                .WithOptional(e => e.CustomRule)
                .WillCascadeOnDelete(false);


            modelBuilder.Entity<UserGameDataSnapshotEntity>()
                .ToTable("UserGameDataSnapshot")
                .HasKey(_ => _.UserId);

            //.HasKey(_ => new {_.UserId, _.CounterId});
        }
    }
}
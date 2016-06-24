using System.Data.Entity;

namespace EntityFX.EconomicsArcade.DataAccess.Model
{
    public class EconomicsArcadeDbContext : DbContext
    {
        public virtual DbSet<CounterEntity> Counters { get; set; }
        public virtual DbSet<FundsDriverEntity> FundsDrivers { get; set; }
        public virtual DbSet<IncrementorEntity> Incrementors { get; set; }
        public virtual DbSet<UserEntity> Users { get; set; }
        public virtual DbSet<UserGameCounterEntity> UserGameCounters { get; set; }
        public virtual DbSet<UserCounterEntity> UserCounters { get; set; }
        public EconomicsArcadeDbContext()
            : base("EconomicsArcadeDbContext")
        {

        }

        public EconomicsArcadeDbContext(string connectionString)
            : base(connectionString)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserEntity>()
                .ToTable("User")
                .HasKey(_ => _.Id);

            modelBuilder.Entity<UserEntity>()
                .Property(_ => _.Email)
                .IsRequired();

            modelBuilder.Entity<CounterEntity>()
                .ToTable("Counter")
                .HasKey(_ => _.Id);

            modelBuilder.Entity<CounterEntity>()
                .Property(e => e.InitialValue)
                .HasPrecision(18, 0);

            modelBuilder.Entity<CounterEntity>()
                .Property(e => e.DelayedValue)
                .HasPrecision(18, 0);

            modelBuilder.Entity<CounterEntity>()
                .HasMany(e => e.UserCounters)
                .WithRequired(e => e.Counter)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<FundsDriverEntity>()
                .ToTable("FundsDriver")
                .HasKey(_ => _.Id);

            modelBuilder.Entity<FundsDriverEntity>()
                .Property(e => e.InitialValue)
                .HasPrecision(18, 0);

            modelBuilder.Entity<FundsDriverEntity>()
                .Property(e => e.UnlockValue)
                .HasPrecision(18, 0);

            modelBuilder.Entity<FundsDriverEntity>()
                .HasMany(e => e.Incrementors)
                .WithRequired(e => e.FundsDriver)
                .WillCascadeOnDelete(false);


            modelBuilder.Entity<IncrementorEntity>()
                .ToTable("Incrementor")
                .HasKey(_ => _.Id);

            modelBuilder.Entity<IncrementorEntity>()
                .Property(e => e.Value)
                .HasPrecision(18, 0);

            modelBuilder.Entity<UserEntity>()
                .HasOptional(e => e.UserGameCounter)
                .WithRequired(e => e.User);

            modelBuilder.Entity<UserEntity>()
                 .HasMany(e => e.UserCounters)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<UserGameCounterEntity>()
                .ToTable("UserGameCounter")
                .HasKey(_ => _.UserId);
            
            modelBuilder.Entity<UserGameCounterEntity>()
                .Property(e => e.TotalFunds)
                .HasColumnType("Money");

            modelBuilder.Entity<UserGameCounterEntity>()
                .Property(e => e.CurrentFunds)
                .HasColumnType("Money");

            modelBuilder.Entity<UserGameCounterEntity>()
                .Property(e => e.DelayedFunds)
                .HasColumnType("Money");

            modelBuilder.Entity<UserCounterEntity>()
                .ToTable("UserCounter");

            modelBuilder.Entity<UserCounterEntity>()
                .Property(e => e.Value)
                .HasColumnType("Money");

            modelBuilder.Entity<UserCounterEntity>()
                .Property(e => e.Bonus)
                .HasColumnType("Money");

            modelBuilder.Entity<UserCounterEntity>()
                .Property(e => e.DelayedValue)
                .HasColumnType("Money");

            //.HasKey(_ => new {_.UserId, _.CounterId});
        }
    }
}

namespace EntityFX.Gdcame.DataAccess.Repository.Ef.MainServer
{
    using System.Data.Entity;

    using EntityFX.Gdcame.DataAccess.Repository.Ef.MainServer.Entities;

    public class DbContext : Common.DbContext
    {
        public virtual DbSet<CounterEntity> Counters { get; set; }
        public virtual DbSet<FundsDriverEntity> FundsDrivers { get; set; }
        public virtual DbSet<IncrementorEntity> Incrementors { get; set; }

        public virtual DbSet<UserGameDataSnapshotEntity> UserGameDataSnapshot { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

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
      
            modelBuilder.Entity<CustomRuleEntity>()
                .ToTable("CustomRule");

            modelBuilder.Entity<CustomRuleEntity>()
                .HasMany(e => e.FundsDrivers)
                .WithOptional(e => e.CustomRule)
                .WillCascadeOnDelete(false);


            modelBuilder.Entity<UserGameDataSnapshotEntity>()
                .ToTable("UserGameDataSnapshot")
                .HasKey(_ => _.UserId);
        }
    }
}
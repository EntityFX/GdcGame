namespace EntityFX.Gdcame.DataAccess.Repository.Ef.MainServer.Entities
{
    using System.ComponentModel.DataAnnotations.Schema;

    public class IncrementorEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public short Type { get; set; }

        public decimal Value { get; set; }

        public int FundsDriverId { get; set; }

        public int? CounterId { get; set; }

        public virtual CounterEntity Counter { get; set; }

        public virtual FundsDriverEntity FundsDriver { get; set; }
    }
}
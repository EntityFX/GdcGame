using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityFX.EconomicsArcade.DataAccess.Model
{
    [Table("Incrementor")]
    public class IncrementorEntity
    {
        [Key]
        public int Id { get; set; }
        public short Type { get; set; }
        public decimal Value { get; set; }
        public int FundsDriverId { get; set; }
        public int CounterId { get; set; }
        [ForeignKey("FK_FundsDriver")]
        public virtual FundsDriverEntity FundsDriver { get; set; }
        [ForeignKey("FK_Counter")]
        public virtual CounterEntity Counter { get; set; }

    }
}

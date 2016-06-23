using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EntityFX.EconomicsArcade.DataAccess.Model;

namespace EntityFX.EconomicsArcade.DataAccess.Model
{
    [Table("UserFundsDriver")]
    public class UserFundsDriver
    {
        [Key]
        public int UserId { get; set; }
        [Key]
        public int FundsDriverId { get; set; }
        public decimal Value { get; set; }
        public int ButCount { get; set; }

        [ForeignKey("FK_User")]
        public virtual UserEntity User { get; set; }

        [ForeignKey("FK_FundsDriver")]
        public virtual FundsDriverEntity FundsDriver { get; set; }
    }
}
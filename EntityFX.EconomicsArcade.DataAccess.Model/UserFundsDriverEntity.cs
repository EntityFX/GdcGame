using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityFX.EconomicsArcade.DataAccess.Model
{
    [Table("UserFundsDriver")]
    public class UserFundsDriverEntity
    {
        [Key]
        public int UserId { get; set; }
        [Key]
        public int FundsDriverId { get; set; }
        public decimal Value { get; set; }
        public int BuyCount { get; set; }
        public DateTime CreateDateTime { get; set; }

        [ForeignKey("FK_User")]
        public virtual UserEntity User { get; set; }

        [ForeignKey("FK_FundsDriver")]
        public virtual FundsDriverEntity FundsDriver { get; set; }
    }
}
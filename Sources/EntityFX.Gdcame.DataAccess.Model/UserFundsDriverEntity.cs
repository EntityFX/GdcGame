using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityFX.Gdcame.DataAccess.Model
{

    public class UserFundsDriverEntity
    {
        [Key]
        //[Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int UserId { get; set; }

        [Key]
        //[Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int FundsDriverId { get; set; }


        public decimal Value { get; set; }
        public int BuyCount { get; set; }
        //public DateTime CreateDateTime { get; set; }


        public virtual UserEntity User { get; set; }


        public virtual FundsDriverEntity FundsDriver { get; set; }
    }
}
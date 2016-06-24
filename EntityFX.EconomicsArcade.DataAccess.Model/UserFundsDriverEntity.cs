using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityFX.EconomicsArcade.DataAccess.Model
{

    public class UserFundsDriverEntity
    {

        public int UserId { get; set; }

        public int FundsDriverId { get; set; }
        public decimal Value { get; set; }
        public int BuyCount { get; set; }
        public DateTime CreateDateTime { get; set; }


        public virtual UserEntity User { get; set; }


        public virtual FundsDriverEntity FundsDriver { get; set; }
    }
}
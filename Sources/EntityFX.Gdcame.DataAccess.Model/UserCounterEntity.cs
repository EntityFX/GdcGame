using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityFX.Gdcame.DataAccess.Model
{

    public class UserCounterEntity
    {

        [Key]
        //[Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int UserId { get; set; }

        [Key]
        //[Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CounterId { get; set; }

        public decimal Value { get; set; }
        public int BonusPercentage { get; set; }
        public int CurrentStepsCount { get; set; }
        public int Inflation { get; set; }
        public int MiningTimeSecondsEllapsed { get; set; }
        public decimal DelayedValue { get; set; }
        public DateTime CreateDateTime { get; set; }


        public virtual UserEntity User { get; set; }

        public virtual CounterEntity Counter { get; set; }
    }
}
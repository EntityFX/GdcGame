using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFX.EconomicsArcade.DataAccess.Model;

namespace EntityFX.EconomicsArcade.DataAccess.Model
{
    [Table("UserCounter")]
    public class UserCounterEntity
    {
        [Key]
        public int UserId { get; set; }
        [Key]
        public int CounterId { get; set; }

        public decimal Value { get; set; }
        public int BonusPercentage { get; set; }
        public decimal Bonus { get; set; }
        public int Inflation { get; set; }
        public int MiningTimeSecondsEllapsed { get; set; }
        public decimal DelayedValue { get; set; }

        [ForeignKey("FK_User")]
        public virtual UserEntity User { get; set; }

        [ForeignKey("FK_Counter")]
        public virtual CounterEntity Counter { get; set; }
    }
}
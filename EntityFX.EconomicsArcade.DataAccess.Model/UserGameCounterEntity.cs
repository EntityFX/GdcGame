using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFX.EconomicsArcade.DataAccess.Model
{
    [Table("UserGameCounter")]
    public class UserGameCounterEntity
    {
        [Key]
        public int UserId { get; set; }
        public decimal TotalFunds { get; set; }
        public int ManualStepsCount { get; set; }
        public int AutomaticStepsCount { get; set; }
        public decimal CategoryFunds { get; set; }
        public decimal DelayedFunds { get; set; }

        [ForeignKey("FK_User")]
        public virtual UserEntity User { get; set; }

    }
}
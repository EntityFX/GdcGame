using System;

namespace EntityFX.Gdcame.DataAccess.Model
{

    public partial class UserGameCounterEntity
    {
        public int UserId { get; set; }

        public decimal TotalFunds { get; set; }
        public decimal CurrentFunds { get; set; }

        public int ManualStepsCount { get; set; }

        public int AutomaticStepsCount { get; set; }

        public DateTime CreateDateTime { get; set; }

        public DateTime? UpdateDateTime { get; set; }

        public decimal DelayedFunds { get; set; }

        public virtual UserEntity User { get; set; }
    }
}
using System;
using System.Collections.Generic;

namespace EntityFX.Gdcame.DataAccess.Model
{

    public partial class UserEntity
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public string Secret { get; set; }

        public string Salt { get; set; }

        public bool IsAdmin { get; set; }

        public DateTime CreateDateTime { get; set; }

        public DateTime? UpdateDateTime { get; set; }

        public virtual UserGameCounterEntity UserGameCounter { get; set; }

        public virtual ICollection<UserCounterEntity> UserCounters { get; set; }

        public virtual ICollection<UserFundsDriverEntity> UserFundsDrivers { get; set; }

        public virtual ICollection<UserCustomRuleEntity> UserCustomRules { get; set; }
    }
}

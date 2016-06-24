using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EntityFX.EconomicsArcade.DataAccess.Model
{

    public partial class UserEntity
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public string Secret { get; set; }

        public string Salt { get; set; }

        public DateTime CreateDateTime { get; set; }

        public DateTime? UpdateDateTime { get; set; }

        public virtual UserGameCounterEntity UserGameCounter { get; set; }

        public virtual ICollection<UserCounterEntity> UserCounters { get; set; }
    }
}

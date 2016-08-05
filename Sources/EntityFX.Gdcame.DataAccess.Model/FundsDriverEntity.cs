using System.Collections.Generic;

namespace EntityFX.Gdcame.DataAccess.Model
{
    public partial class FundsDriverEntity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public FundsDriverEntity()
        {
            Incrementors = new HashSet<IncrementorEntity>();
            UserFundsDrivers = new HashSet<UserFundsDriverEntity>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal InitialValue { get; set; }

        public decimal UnlockValue { get; set; }

        public short InflationPercent { get; set; }

        public string Picture { get; set; }

        public int? CustomRuleId { get; set; }

        public virtual CustomRuleEntity CustomRule { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<IncrementorEntity> Incrementors { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserFundsDriverEntity> UserFundsDrivers { get; set; }
    }
}

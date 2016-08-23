using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace EntityFX.Gdcame.DataAccess.Model.Ef
{
    public class FundsDriverEntity
    {
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public FundsDriverEntity()
        {
            Incrementors = new HashSet<IncrementorEntity>();
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

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<IncrementorEntity> Incrementors { get; set; }
    }
}
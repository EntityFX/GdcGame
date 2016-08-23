using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace EntityFX.Gdcame.DataAccess.Model.Ef
{
    public class CustomRuleEntity
    {
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CustomRuleEntity()
        {
            FundsDrivers = new HashSet<FundsDriverEntity>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FundsDriverEntity> FundsDrivers { get; set; }
    }
}
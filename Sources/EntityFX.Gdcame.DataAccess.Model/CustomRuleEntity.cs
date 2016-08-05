using System.Collections.Generic;

namespace EntityFX.Gdcame.DataAccess.Model
{
    public class CustomRuleEntity
    {
        public int Id { get; set; }

        public string Name { get; set; } 

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FundsDriverEntity> FundsDrivers { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserCustomRuleEntity> UserCustomRules { get; set; }

                [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CustomRuleEntity()
        {
            FundsDrivers = new HashSet<FundsDriverEntity>();
            UserCustomRules = new HashSet<UserCustomRuleEntity>();
        }
    }
}
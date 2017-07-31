namespace EntityFX.Gdcame.DataAccess.Repository.Ef.MainServer.Entities
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    public class CustomRuleEntity
    {
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CustomRuleEntity()
        {
            this.FundsDrivers = new HashSet<FundsDriverEntity>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FundsDriverEntity> FundsDrivers { get; set; }
    }
}
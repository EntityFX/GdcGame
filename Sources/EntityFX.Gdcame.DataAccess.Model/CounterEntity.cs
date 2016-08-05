using System.Collections.Generic;

namespace EntityFX.Gdcame.DataAccess.Model
{

    public partial class CounterEntity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CounterEntity()
        {
            Incrementors = new HashSet<IncrementorEntity>();
            UserCounters = new HashSet<UserCounterEntity>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public decimal InitialValue { get; set; }

        public bool UseInAutostep { get; set; }

        public int InflationIncreaseSteps { get; set; }

        public int? MiningTimeSeconds { get; set; }

        public int Type { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<IncrementorEntity> Incrementors { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserCounterEntity> UserCounters { get; set; }
    }
}

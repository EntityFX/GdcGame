﻿using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace EntityFX.Gdcame.DataAccess.Model.Ef
{
    public class CounterEntity
    {
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CounterEntity()
        {
            Incrementors = new HashSet<IncrementorEntity>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public decimal InitialValue { get; set; }

        public bool UseInAutostep { get; set; }

        public int InflationIncreaseSteps { get; set; }

        public int? MiningTimeSeconds { get; set; }

        public int Type { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<IncrementorEntity> Incrementors { get; set; }
    }
}
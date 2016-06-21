using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityFX.EconomicsArcade.DataAccess.Model
{
    [Table("Counter")]
    public class CounterEntity
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal InitialValue { get; set; }
        public bool UseInAutoStep { get; set; }
        public int InflationIncreaseSteps { get; set; }
        public int? MiningTimeSeconds { get; set; }
        public decimal? DelayedValue { get; set; }
        public int Type { get; set; }
        public virtual ICollection<IncrementorEntity> Incrementors { get; set; }
    }
}

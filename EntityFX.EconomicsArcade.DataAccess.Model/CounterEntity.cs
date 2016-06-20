using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFX.EconomicsArcade.DataAccess.Model
{
    [Table("Counter")]
    class CounterEntity
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
    }
}

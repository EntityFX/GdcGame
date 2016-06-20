using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFX.EconomicsArcade.DataAccess.Model
{
    [Table("FundsDriver")]
    class FundsDriverEntity
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal InitialValue { get; set; }
        public decimal UnlockValue { get; set; }
        public short InflationPercent { get; set; }
    }
}

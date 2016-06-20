using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFX.EconomicsArcade.DataAccess.Model
{
    [Table("Incrementor")]
    class IncrementorEntity
    {
        [Key]
        public int Id { get; set; }
        public short Type { get; set; }
        public decimal Value { get; set; }
        public int FundsDriverId { get; set; }
    }
}

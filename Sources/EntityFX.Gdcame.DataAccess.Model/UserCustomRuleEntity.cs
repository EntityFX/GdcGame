using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityFX.Gdcame.DataAccess.Model
{
    public class UserCustomRuleEntity
    {
        [Key]
        //[Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int UserId { get; set; }

        [Key]
        //[Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CustomRuleId { get; set; }

        [Key]
        //[Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int FundsDriverId { get; set; }

        public int? CurrentIndex { get; set; }

        public virtual UserEntity User { get; set; }

        public virtual CustomRuleEntity CustomRule { get; set; }

        public virtual FundsDriverEntity FundsDriver { get; set; }

    }
}
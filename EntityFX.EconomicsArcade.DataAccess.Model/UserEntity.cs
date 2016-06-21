using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EntityFX.EconomicsArcade.DataAccess.Model
{
    [Table("User")]
    public class UserEntity
    {
        [Key]
        public int Id { get; set; }

        [Index(IsUnique=true)]
        public string Email { get; set; }

        public DateTime CreateDateTime { get; set; }

        public DateTime? UpdateDateTime { get; set; }
    }
}

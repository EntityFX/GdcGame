using System;

namespace EntityFX.Gdcame.DataAccess.Model.Ef
{

    public partial class UserEntity
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public string Secret { get; set; }

        public string Salt { get; set; }

        public bool IsAdmin { get; set; }

        public DateTime CreateDateTime { get; set; }

        public DateTime? UpdateDateTime { get; set; }
    }
}

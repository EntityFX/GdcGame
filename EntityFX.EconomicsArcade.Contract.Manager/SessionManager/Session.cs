using System;

namespace EntityFX.EconomicsArcade.Contract.Manager.SessionManager
{
    public class Session
    {
        public Guid SessionIdentifier { get; set; }

        public string Login { get; set; }

        public int UserId { get; set; }
    }
}

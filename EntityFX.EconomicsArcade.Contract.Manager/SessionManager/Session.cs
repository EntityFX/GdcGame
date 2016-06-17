using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFX.EconomicsArcade.Contract.Manager.SessionManager
{
    public class Session
    {
        public Guid SessionIdentifier { get; set; }

        public string Login { get; set; }
    }
}

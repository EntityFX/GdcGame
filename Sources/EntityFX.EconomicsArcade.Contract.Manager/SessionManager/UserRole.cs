using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EntityFX.EconomicsArcade.Contract.Manager.SessionManager
{
    [DataContract]
    public enum UserRole
    {
        [EnumMember]
        GenericUser,
        [EnumMember]
        Admin
    }
}

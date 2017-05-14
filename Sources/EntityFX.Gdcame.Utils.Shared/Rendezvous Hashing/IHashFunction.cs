using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFX.Gdcame.Utils.Shared.Rendezvous_Hashing
{
    interface IHashFunction
    {
        long GetHash(String valueToHash);
    }
}

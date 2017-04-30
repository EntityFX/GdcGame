using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFX.Gdcame.Utils.Shared
{
    interface IHashFunction
    {
        long GetHash(String valueToHash);
    }
}

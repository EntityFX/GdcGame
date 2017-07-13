using System;

namespace EntityFX.Gdcame.Utils.Common.Hashing
{
    interface IHashFunction
    {
        long GetHash(String valueToHash);
    }
}

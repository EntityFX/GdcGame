using System;

namespace EntityFX.Gdcame.Utils.Hashing
{
    interface IHashFunction
    {
        long GetHash(String valueToHash);
    }
}

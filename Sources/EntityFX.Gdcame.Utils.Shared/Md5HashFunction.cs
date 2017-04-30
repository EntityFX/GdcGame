using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EntityFX.Gdcame.Utils.Shared
{
    class Md5HashFunction : IHashFunction
    {
        public long GetHash(String valueToHash)
        {
            // step 1, calculate MD5 hash from input
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(valueToHash);
            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return StringToLong(sb.ToString());
        }

        private long StringToLong(String str)
        {
            return Convert.ToInt64(str);
        }
        
    }
}

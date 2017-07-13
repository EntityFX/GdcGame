using System;
using System.Security.Cryptography;
using System.Text;

namespace EntityFX.Gdcame.Utils.Common.Hashing
{
    class Md5HashFunction : IHashFunction
    {
        public long GetHash(String valueToHash)
        {
            // step 1, calculate MD5 hash from input
            MD5 md5 = MD5.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes(valueToHash);
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
            //            return Convert.ToInt64(str);
            Int64 value = 0;
            Int64 _2_Pow_4Pos_ByModulo = 1;
            for (int i = str.Length - 1; i >= 0; i--)
            {
                int x = 0;
                char c = str[i];
                if ((c >= 'a') && (c <= 'f'))
                {
                    x = c - 'a' + 10;
                }
                else if ((c >= '0') && (c <= '9'))
                {
                    x = c - '0';
                }
            
                value = (value + x * _2_Pow_4Pos_ByModulo);
                _2_Pow_4Pos_ByModulo = (_2_Pow_4Pos_ByModulo << 4);
            }
            
            return (int)value;
        }

    }
}

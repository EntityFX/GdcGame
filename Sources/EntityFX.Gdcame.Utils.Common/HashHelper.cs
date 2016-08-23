using System.Security.Cryptography;
using System.Text;
using EntityFX.Gdcame.Infrastructure.Common;

namespace EntityFX.Gdcame.Utils.Common
{
    public class HashHelper : IHashHelper
    {
        public string GetHashedString(string input)
        {
            var inputBytes = Encoding.ASCII.GetBytes(input + "_gdcame");
            using (var md5 = MD5.Create())
            {
                var hash = md5.ComputeHash(inputBytes);
                return ToHex(hash);
            }
        }

        private static string ToHex(byte[] bytes)
        {
            var c = new char[bytes.Length*2];

            byte b;

            for (int bx = 0, cx = 0; bx < bytes.Length; ++bx, ++cx)
            {
                b = ((byte) (bytes[bx] >> 4));
                c[cx] = (char) (b > 9 ? b + 0x37 + 0x20 : b + 0x30);

                b = ((byte) (bytes[bx] & 0x0F));
                c[++cx] = (char) (b > 9 ? b + 0x37 + 0x20 : b + 0x30);
            }

            return new string(c);
        }
    }
}
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using EntityFX.Gdcame.DataAccess.Service;
using EntityFX.Gdcame.Infrastructure.Common;
using EntityFX.Gdcame.Manager.Common;
using EntityFX.Gdcame.Manager.Contract.Common.ServerManager;

namespace EntityFX.Gdcame.Utils.Common.Hashing
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

        //        public int GetModuloOfUserIdHash(string userId, int modulo)
        //        {
        //            Int64 value = 0;
        //            Int64 _2_Pow_4Pos_ByModulo = 1;
        //            for (int i = userId.Length - 1; i >= 0; i--)
        //            {
        //                int x = 0;
        //                char c = userId[i];
        //                if ((c >= 'a') && (c <= 'f'))
        //                {
        //                    x = c - 'a' + 10;
        //                }
        //                else if ((c >= '0') && (c <= '9'))
        //                {
        //                    x = c - '0';
        //                }
        //
        //                value = (value + x * _2_Pow_4Pos_ByModulo) % modulo;
        //                _2_Pow_4Pos_ByModulo = (_2_Pow_4Pos_ByModulo << 4) % modulo;
        //            }
        //
        //            return (int)value;
        //        }

        private RendezvousHash rendezvousHash;

        public HashHelper()
        {
            IHashFunction hasher = new Md5HashFunction();
            rendezvousHash = new RendezvousHash(hasher);
        }

        public int GetServerNumberByRendezvousHashing(string userId)
        {
            IServerManager serverManager = new ServerManager(new ServerDataAccessService());//TODO:How to get it?
            string[] servers = serverManager.GetServers();

            List<string> serversList = new List<string>(servers);
            RendezvousHash.Logger(string.Format("Searching server for userId = [{0}]. Servers count is {1}.", userId, serversList.Count));
            return rendezvousHash.DetermineResponsibleNode(userId, serversList).ServerNumber;
        }
    }
}
using System.Diagnostics;
using EntityFX.Gdcame.Infrastructure.Common;
using EntityFX.Gdcame.Utils.Hashing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EntityFx.Gdcame.Test.Unit
{
    [TestClass]
    public class HashTest
    {
        [TestMethod]
        public void TestHash()
        {
            IHashHelper hh = new HashHelper();
            Debug.WriteLine(hh.GetHashedString("admin"));

            // TODO: Use Rendezvous Hashing algorithm.
            // Debug.WriteLine(hh.GetModuloOfUserIdHash(hh.GetHashedString("admin"), 4));
            // todo:remove GetHashedString
            Debug.WriteLine(hh.GetServerNumberByUserId(
            new[]{
                "127.0.0.1",
                "127.0.0.2"
            }
            , hh.GetHashedString("admin")));
        }
    }
}
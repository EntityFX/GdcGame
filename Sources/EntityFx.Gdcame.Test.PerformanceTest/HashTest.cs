using System.Diagnostics;
using EntityFX.Gdcame.Infrastructure.Common;
using EntityFX.Gdcame.Utils.Common;
using EntityFX.Gdcame.Utils.Shared;
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

            //TODO: Use Rendezvous Hashing algorithm.
            Debug.WriteLine(hh.GetModuloOfUserIdHash(hh.GetHashedString("admin"), 4));
        }
    }
}
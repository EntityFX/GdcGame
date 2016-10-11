using System.Diagnostics;
using EntityFX.Gdcame.Infrastructure.Common;
using EntityFX.Gdcame.Utils.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EntityFx.Gdcame.Test.PerformanceTest
{
    [TestClass]
    public class HashTest
    {
        [TestMethod]
        public void TestHash()
        {
            IHashHelper hh = new HashHelper();
            Debug.WriteLine(hh.GetHashedString("admin"));

            Debug.WriteLine(hh.GetModuloOfUserIdHash(hh.GetHashedString("admin"), 4));
        }
    }
}
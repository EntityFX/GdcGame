using EntityFX.Gdcame.Infrastructure.Common;
using EntityFX.Gdcame.Utils.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFx.Gdcame.Test.PerformanceTest
{
    [TestClass]
    public class HashTest
    {
        [TestMethod]
        public void TestHash()
        {
            IHashHelper hh = new HashHelper();
            Debug.WriteLine(hh.GetHashedString("entityfx"));
        }
    }
}

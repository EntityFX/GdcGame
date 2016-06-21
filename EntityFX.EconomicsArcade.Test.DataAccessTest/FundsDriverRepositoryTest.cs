using EntityFX.EconomicsArcade.Utils.ClientProxy.DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFX.EconomicsArcade.Test.DataAccessTest
{
    [TestClass]
    public class FundsDriverRepositoryTest
    {
        [TestMethod]
        public void TestGelAllFundsDrivers()
        {
            using (var proxyFactory = new GameDataDataAccessProxyFactory())
            {
                var proxy = proxyFactory.OpenChannel(new Uri("net.tcp://localhost/EntityFX.EconomicsArcade.DataAccess:8777/EntityFX.EconomicsArcade.Contract.DataAccess.GameData.IGameDataDataAccessService"));
                var user = proxy.GetGameData();
                Assert.IsNotNull(user);
            }
        }
    }
}

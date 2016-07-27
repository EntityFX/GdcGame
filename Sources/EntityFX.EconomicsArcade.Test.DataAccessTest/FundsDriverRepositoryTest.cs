using EntityFX.EconomicsArcade.Contract.DataAccess.GameData;
using EntityFX.EconomicsArcade.Infrastructure.Service;
using EntityFX.EconomicsArcade.Utils.ClientProxy.DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace EntityFX.EconomicsArcade.Test.DataAccessTest
{
    [TestClass]
    public class FundsDriverRepositoryTest
    {
        [TestMethod]
        public void TestGelAllFundsDrivers()
        {
            var proxy =
    new GameDataRetrieveDataAccessClient<NetTcpProxy<IGameDataRetrieveDataAccessService>>(
        "net.tcp://localhost/EntityFX.EconomicsArcade.DataAccess:8777/EntityFX.EconomicsArcade.Contract.DataAccess.GameData.IGameDataRetrieveDataAccessService");

            var user = proxy.GetGameData(1);
            Assert.IsNotNull(user);
        }
    }
}

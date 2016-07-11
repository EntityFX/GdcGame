﻿using EntityFX.EconomicsArcade.Utils.ClientProxy.DataAccess;
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
            using (var proxyFactory = new GameDataRetrieveDataAccessProxy())
            {
                var proxy = proxyFactory.CreateChannel(new Uri("net.tcp://localhost/EntityFX.EconomicsArcade.DataAccess:8777/EntityFX.EconomicsArcade.Contract.DataAccess.GameData.IGameDataRetrieveDataAccessService"));
                var user = proxy.GetGameData(1);
                proxyFactory.CloseChannel();
                Assert.IsNotNull(user);
            }
        }
    }
}

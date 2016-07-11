using System;
using EntityFX.EconomicsArcade.Utils.ClientProxy.DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EntityFX.EconomicsArcade.Test.DataAccessTest
{
    [TestClass]
    public class UserRepositoryTest
    {
        [TestMethod]
        public void TestCreateUser()
        {
            using (var proxyFactory = new UserDataAccessProxyFactoy())
            {
                
                var channel = proxyFactory.CreateChannel(new Uri("net.tcp://localhost/EntityFX.EconomicsArcade.DataAccess:8777/EntityFX.EconomicsArcade.Contract.DataAccess.User.IUserDataAccessService"));
                channel.Create(new Contract.DataAccess.User.User()
                {
                    Email = "vasya2"
                });
                proxyFactory.CloseChannel();
            }

        }

        [TestMethod]
        public void TestFindByIdUser()
        {
            using (var proxyFactory = new UserDataAccessProxyFactoy())
            {
                var channel = proxyFactory.CreateChannel(new Uri("net.tcp://localhost/EntityFX.EconomicsArcade.DataAccess:8777/EntityFX.EconomicsArcade.Contract.DataAccess.User.IUserDataAccessService"));
                var user = channel.FindById(1);
                proxyFactory.CloseChannel();
                Assert.IsNotNull(user);
            }
        }
    }
}

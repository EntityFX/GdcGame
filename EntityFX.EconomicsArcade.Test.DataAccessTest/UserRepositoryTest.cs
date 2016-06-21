using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EntityFX.EconomicsArcade.Utils.ClientProxy.Manager;

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
                var proxy = proxyFactory.OpenChannel(new Uri("net.tcp://localhost/EntityFX.EconomicsArcade.DataAccess:8777/EntityFX.EconomicsArcade.Contract.DataAccess.User.IUserDataAccessService"));
                proxy.Create(new Contract.DataAccess.User.User()
                {
                    Email = "vasya2"
                });
            }

        }

        [TestMethod]
        public void TestFindByIdUser()
        {
            using (var proxyFactory = new UserDataAccessProxyFactoy())
            {
                var proxy = proxyFactory.OpenChannel(new Uri("net.tcp://localhost/EntityFX.EconomicsArcade.DataAccess:8777/EntityFX.EconomicsArcade.Contract.DataAccess.User.IUserDataAccessService"));
                var user = proxy.FindById(1);
                Assert.IsNotNull(user);
            }
        }
    }
}

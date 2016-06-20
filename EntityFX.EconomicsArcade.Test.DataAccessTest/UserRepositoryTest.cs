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
            using (var proxyFactory = new UserRepositoryProxyFactoy())
            {

                var proxy = proxyFactory.OpenChannel(new Uri("net.tcp://localhost/EntityFX.EconomicsArcade.DataAccess:8777/EntityFX.EconomicsArcade.Contract.DataAccess.User.IUserRepository"));
                proxy.Create(new Contract.DataAccess.User.User()
                {
                    Email = "vasya"
                });
            }

        }
    }
}

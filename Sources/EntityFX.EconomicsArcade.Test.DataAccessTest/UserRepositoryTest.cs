using System;
using EntityFX.EconomicsArcade.Contract.DataAccess.User;
using EntityFX.EconomicsArcade.Infrastructure.Service;
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
            var proxyFactory =
                new UserDataAccessClient<NetTcpProxy<IUserDataAccessService>>(
                    "net.tcp://localhost/EntityFX.EconomicsArcade.DataAccess:8777/EntityFX.EconomicsArcade.Contract.DataAccess.User.IUserDataAccessService");
            {
                
                proxyFactory.Create(new Contract.DataAccess.User.User()
                {
                    Email = "vasya2"
                });

            }

        }

        [TestMethod]
        public void TestFindByIdUser()
        {
            var proxyFactory =
                new UserDataAccessClient<NetTcpProxy<IUserDataAccessService>>(
                    "net.tcp://localhost/EntityFX.EconomicsArcade.DataAccess:8777/EntityFX.EconomicsArcade.Contract.DataAccess.User.IUserDataAccessService");
                var user = proxyFactory.FindById(1);
                Assert.IsNotNull(user);
            }
        }
    }


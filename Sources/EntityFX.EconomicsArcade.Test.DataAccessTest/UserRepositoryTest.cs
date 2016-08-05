using EntityFX.Gdcame.DataAccess.Contract.User;
using EntityFX.Gdcame.Infrastructure.Service.NetTcp;
using EntityFX.Gdcame.Utils.ClientProxy.DataAccess;
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
                
                proxyFactory.Create(new User()
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


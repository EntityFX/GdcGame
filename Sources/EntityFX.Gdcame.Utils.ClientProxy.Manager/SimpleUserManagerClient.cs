using System;
using EntityFX.Gdcame.Infrastructure.Service.Bases;
using EntityFX.Gdcame.Manager.Contract.UserManager;

namespace EntityFX.Gdcame.Utils.ClientProxy.Manager
{
    public class SimpleUserManagerClient<TInfrastructureProxy> : ISimpleUserManager
                where TInfrastructureProxy : InfrastructureProxy<ISimpleUserManager>, new()
    {
        private readonly Uri _endpointAddress;// = new Uri("net.tcp://localhost:8555/EntityFX.EconomicsArcade.Manager/EntityFX.EconomicsArcade.Contract.Manager.UserManager.ISimpleUserManager");

        public SimpleUserManagerClient(string endpointAddress)
        {
            _endpointAddress = new Uri(endpointAddress);
        }

        public bool Exists(string login)
        {
            using (var proxy = new TInfrastructureProxy())
            {
                var channel = proxy.CreateChannel(_endpointAddress);
                proxy.ApplyContextScope();
                var res = channel.Exists(login);
                proxy.CloseChannel();
                return res;
            }
        }

        public UserData FindById(int id)
        {
            using (var proxy = new TInfrastructureProxy())
            {
                var channel = proxy.CreateChannel(_endpointAddress);
                proxy.ApplyContextScope();
                var res = channel.FindById(id);
                proxy.CloseChannel();
                return res;
            }
        }

        public UserData Find(string login)
        {
            using (var proxy = new TInfrastructureProxy())
            {
                var channel = proxy.CreateChannel(_endpointAddress);
                proxy.ApplyContextScope();
                var res = channel.Find(login);
                proxy.CloseChannel();
                return res;
            }
        }

        public void Create(UserData login)
        {
            using (var proxy = new TInfrastructureProxy())
            {
                var channel = proxy.CreateChannel(_endpointAddress);
                proxy.ApplyContextScope();
                channel.Create(login);
                proxy.CloseChannel();
            }
        }

        public void Delete(int id)
        {
            using (var proxy = new TInfrastructureProxy())
            {
                var channel = proxy.CreateChannel(_endpointAddress);
                proxy.ApplyContextScope();
                channel.Delete(id);
                proxy.CloseChannel();
            }
        }
    }
}
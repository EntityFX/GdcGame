using System;
using EntityFX.EconomicsArcade.Contract.Manager.UserManager;

namespace EntityFX.EconomicsArcade.Utils.ClientProxy.Manager
{
    public class SimpleUserManagerClient : ISimpleUserManager
    {
        private readonly Uri _endpointAddress = new Uri("net.tcp://localhost:8555/EntityFX.EconomicsArcade.Manager/EntityFX.EconomicsArcade.Contract.Manager.UserManager.ISimpleUserManager");
        
        public bool Exists(string login)
        {
            using (var proxy = new SimpleUserManagerProxy())
            {
                var channel = proxy.CreateChannel(_endpointAddress);
                proxy.ApplyContextScope();
                var res = channel.Exists(login);
                proxy.CloseChannel();
                return res;
            }
        }

        public void Create(string login)
        {
            using (var proxy = new SimpleUserManagerProxy())
            {
                var channel = proxy.CreateChannel(_endpointAddress);
                proxy.ApplyContextScope();
                channel.Create(login);
                proxy.CloseChannel();
            }
        }
    }
}
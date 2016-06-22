using System;
using EntityFX.EconomicsArcade.Contract.DataAccess.User;

namespace EntityFX.EconomicsArcade.Utils.ClientProxy.DataAccess
{
    public class UserDataAccessClient : IUserDataAccessService
    {
        private const string Endpoint =
    "net.tcp://localhost:8777/EntityFX.EconomicsArcade.DataAccess/EntityFX.EconomicsArcade.Contract.DataAccess.User.IUserDataAccessService";
        
        
        public int Create(User user)
        {
            int result;
            using (var proxy = new UserDataAccessProxyFactoy())
            {
                var channel = proxy.CreateChannel(new Uri(Endpoint));
                result = channel.Create(user);
                proxy.CloseChannel();
            }
            return result;
        }

        public void Update(User user)
        {
            using (var proxy = new UserDataAccessProxyFactoy())
            {
                var channel = proxy.CreateChannel(new Uri(Endpoint));
                channel.Update(user);
                proxy.CloseChannel();
            }
        }

        public void Delete(int userId)
        {
            using (var proxy = new UserDataAccessProxyFactoy())
            {
                var channel = proxy.CreateChannel(new Uri(Endpoint));
                channel.Delete(userId);
                proxy.CloseChannel();
            }
        }

        public User FindById(int userId)
        {
            User result;
            using (var proxy = new UserDataAccessProxyFactoy())
            {
                var channel = proxy.CreateChannel(new Uri(Endpoint));
                result = channel.FindById(userId);
                proxy.CloseChannel();
            }
            return result;
        }

        public User FindByName(string name)
        {
            User result;
            using (var proxy = new UserDataAccessProxyFactoy())
            {
                var channel = proxy.CreateChannel(new Uri(Endpoint));
                result = channel.FindByName(name);
                proxy.CloseChannel();
            }
            return result;
        }

        public User[] FindAll()
        {
            User[] result;
            using (var proxy = new UserDataAccessProxyFactoy())
            {
                var channel = proxy.CreateChannel(new Uri(Endpoint));
                result = channel.FindAll();
                proxy.CloseChannel();
            }
            return result;
        }
    }
}
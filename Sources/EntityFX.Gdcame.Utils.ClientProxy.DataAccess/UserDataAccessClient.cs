using System;
using EntityFX.Gdcame.DataAccess.Contract.User;
using EntityFX.Gdcame.Infrastructure.Service.Bases;

namespace EntityFX.Gdcame.Utils.ClientProxy.DataAccess
{
    public class UserDataAccessClient<TInfrastructureProxy> : IUserDataAccessService
                where TInfrastructureProxy : InfrastructureProxy<IUserDataAccessService>, new()
    {
    //"net.tcp://localhost:8777/EntityFX.EconomicsArcade.DataAccess/EntityFX.EconomicsArcade.Contract.DataAccess.User.IUserDataAccessService";

        private readonly Uri _endpoint;

        public UserDataAccessClient(string endpoint)
        {
            _endpoint = new Uri(endpoint);
        }

        public int Create(User user)
        {
            int result;
            using (var proxy = new TInfrastructureProxy())
            {
                var channel = proxy.CreateChannel(_endpoint);
                result = channel.Create(user);
                proxy.CloseChannel();
            }
            return result;
        }

        public void Update(User user)
        {
            using (var proxy = new TInfrastructureProxy())
            {
                var channel = proxy.CreateChannel(_endpoint);
                channel.Update(user);
                proxy.CloseChannel();
            }
        }

        public void Delete(int userId)
        {
            using (var proxy = new TInfrastructureProxy())
            {
                var channel = proxy.CreateChannel(_endpoint);
                channel.Delete(userId);
                proxy.CloseChannel();
            }
        }

        public User FindById(int userId)
        {
            User result;
            using (var proxy = new TInfrastructureProxy())
            {
                var channel = proxy.CreateChannel(_endpoint);
                result = channel.FindById(userId);
                proxy.CloseChannel();
            }
            return result;
        }

        public User FindByName(string name)
        {
            User result;
            using (var proxy = new TInfrastructureProxy())
            {
                var channel = proxy.CreateChannel(_endpoint);
                result = channel.FindByName(name);
                proxy.CloseChannel();
            }
            return result;
        }

        public User[] FindAll()
        {
            User[] result;
            using (var proxy = new TInfrastructureProxy())
            {
                var channel = proxy.CreateChannel(_endpoint);
                result = channel.FindAll();
                proxy.CloseChannel();
            }
            return result;
        }
    }
}
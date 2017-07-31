namespace EntityFX.Gdcame.DataAccess.Repository.Contract.MainServer
{
    using EntityFX.Gdcame.DataAccess.Contract.MainServer.Server;

    public interface IServerRepository
    {
        Server[] FindServers();

        void Create(string server);

        void Delete(string server);
    }
}

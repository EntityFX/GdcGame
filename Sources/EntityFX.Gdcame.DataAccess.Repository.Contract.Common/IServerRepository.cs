namespace EntityFX.Gdcame.DataAccess.Repository.Contract.Common
{
    using EntityFX.Gdcame.DataAccess.Contract.Common.Server;

    public interface IServerRepository
    {
        Server[] FindServers();

        void Create(string server);

        void Delete(string server);
    }
}

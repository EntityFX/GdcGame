namespace EntityFX.Gdcame.DataAccess.Contract.MainServer.Server
{
    public interface IServerDataAccessService
    {
        Server[] GetServers();

        void UpdateServers(string[] servers);

    }
}

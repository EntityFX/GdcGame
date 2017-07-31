namespace EntityFX.Gdcame.DataAccess.Contract.Common.Server
{
    public interface IServerDataAccessService
    {
        Server[] GetServers();

        void UpdateServers(string[] servers);

    }
}

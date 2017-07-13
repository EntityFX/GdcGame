using System.Threading.Tasks;
using EntityFX.Gdcame.Common.Application.Model;

namespace EntityFX.Gdcame.Application.Contract.Controller.Common
{
    public interface IServerController
    {
        Task<ServerInfoModel> GetServersInfo();
        //TODO: add UpdateServersList(ServerInfoModel) that will start NodeDataUpdateJob using WorkersManager.

        string Echo(string text);
        string EchoAuth(string text);

        //TODO: add TransferUsersData(IList<GameWithUserId> gamesWithUserIds) that will start create users with user Games (using IGameDataPersister) and Start Games using StartGame method of GameSessions.
        //TODO: move TransferUsersData(IList<GameWithUserId> gamesWithUserIds) into separate IUserDataTransfer interface that will be inherited by IServerController and located in Manager layer
    }
}

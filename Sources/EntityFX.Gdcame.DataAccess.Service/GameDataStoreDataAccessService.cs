namespace EntityFX.Gdcame.DataAccess.Service.MainServer
{
    using EntityFX.Gdcame.DataAccess.Contract.MainServer.GameData;
    using EntityFX.Gdcame.DataAccess.Repository.Contract.MainServer;
    using EntityFX.Gdcame.Contract.MainServer.Store;

    public class GameDataStoreDataAccessDocumentService : IGameDataStoreDataAccessService
    {
        private readonly IUserGameSnapshotRepository _userGameSnapshotRepository;

        public GameDataStoreDataAccessDocumentService(IUserGameSnapshotRepository userGameSnapshotRepository)
        {
            this._userGameSnapshotRepository = userGameSnapshotRepository;
        }

        public void StoreGameDataForUsers(StoredGameDataWithUserId[] listOfGameDataWithUserId)
        {
            this._userGameSnapshotRepository.CreateOrUpdateUserGames(listOfGameDataWithUserId);
            
            ///////var userGame = _userGameSnapshotRepository.FindByUserId(new GetUserGameSnapshotByIdCriterion(userId));

            ///////if (userGame == null)
            ///////{
            ///////    _userGameSnapshotRepository.CreateForUser(userId, gameData);
            ///////}
            ///////else
            ///////{
            ///////    _userGameSnapshotRepository.UpdateForUser(userId, gameData);
            ///////}
        }
    }
}
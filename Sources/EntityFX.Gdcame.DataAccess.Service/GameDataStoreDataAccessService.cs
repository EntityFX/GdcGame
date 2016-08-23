using EntityFX.Gdcame.DataAccess.Contract.GameData;
using EntityFX.Gdcame.DataAccess.Contract.GameData.Store;
using EntityFX.Gdcame.DataAccess.Repository.Contract;
using EntityFX.Gdcame.DataAccess.Repository.Contract.Criterions.UserGameSnapshot;

namespace EntityFX.Gdcame.DataAccess.Service
{
    public class GameDataStoreDataAccessDocumentService : IGameDataStoreDataAccessService
    {
        private readonly IUserGameSnapshotRepository _userGameSnapshotRepository;

        public GameDataStoreDataAccessDocumentService(IUserGameSnapshotRepository userGameSnapshotRepository)
        {
            _userGameSnapshotRepository = userGameSnapshotRepository;
        }

        public void StoreGameDataForUser(string userId, StoredGameData gameData)
        {
            var userGame = _userGameSnapshotRepository.FindByUserId(new GetUserGameSnapshotByIdCriterion(userId));

            if (userGame == null)
            {
                _userGameSnapshotRepository.CreateForUser(userId, gameData);
            }
            else
            {
                _userGameSnapshotRepository.UpdateForUser(userId, gameData);
            }
        }
    }
}
using System.Linq;
using EntityFX.Gdcame.Common.Contract;
using EntityFX.Gdcame.DataAccess.Contract.GameData;
using EntityFX.Gdcame.DataAccess.Repository;
using EntityFX.Gdcame.DataAccess.Repository.Criterions.UserCounter;
using EntityFX.Gdcame.DataAccess.Repository.Criterions.UserCustomRuleInfo;
using EntityFX.Gdcame.DataAccess.Repository.Criterions.UserFundsDriver;
using EntityFX.Gdcame.DataAccess.Repository.Criterions.UserGameCounter;
using EntityFX.Gdcame.DataAccess.Repository.Criterions.UserGameSnapshot;
using EntityFX.Gdcame.Infrastructure.Common;

namespace EntityFX.Gdcame.DataAccess.Service
{
    public class GameDataStoreDataAccessDocumentService : IGameDataStoreDataAccessService
    {
        private readonly IUserGameSnapshotRepository _userGameSnapshotRepository;

        public GameDataStoreDataAccessDocumentService(IUserGameSnapshotRepository userGameSnapshotRepository)
        {
            _userGameSnapshotRepository = userGameSnapshotRepository;
        }

        public void StoreGameDataForUser(int userId, StoredGameData gameData)
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
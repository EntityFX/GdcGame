using EntityFX.Gdcame.DataAccess.Contract.GameData.Store;
using EntityFX.Gdcame.DataAccess.Model.Ef;
using EntityFX.Gdcame.DataAccess.Repository.Contract;
using EntityFX.Gdcame.DataAccess.Repository.Contract.Criterions.UserGameSnapshot;
using EntityFX.Gdcame.Infrastructure.Repository.UnitOfWork;
using Newtonsoft.Json;

namespace EntityFX.Gdcame.DataAccess.Repository.Ef
{
    public class UserGameSnapshotRepository : IUserGameSnapshotRepository
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        public UserGameSnapshotRepository(IUnitOfWorkFactory unitOfWorkFactory
            )
        {
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        public StoredGameData FindByUserId(GetUserGameSnapshotByIdCriterion criterion)
        {
            using (var uow = _unitOfWorkFactory.Create())
            {
                var findQuery = uow.BuildQuery();
                var entity = findQuery.For<UserGameDataSnapshotEntity>()
                    .With(criterion);
                if (entity == null) return null;
                return Deserialize(entity.Data);
            }
        }

        public void CreateUserGames(StoredGameDataWithUserId[] listOfGameDataWithUserId)
        {

            using (var uow = _unitOfWorkFactory.Create())
            {
                foreach (var gameDataWithUserId in listOfGameDataWithUserId)
                {
                    var userEntity = uow.CreateEntity<UserGameDataSnapshotEntity>();
                    userEntity.Data = Serialize(gameDataWithUserId.StoredGameData);
                    userEntity.UserId = gameDataWithUserId.UserId.ToString();
                }
                uow.Commit();
            }

        }

        public void UpdateUserGames(StoredGameDataWithUserId[] listOfGameDataWithUserId)
        {
            using (var uow = _unitOfWorkFactory.Create())
            {
                foreach (var gameDataWithUserId in listOfGameDataWithUserId)
                {
                    var userEntity = uow.CreateEntity<UserGameDataSnapshotEntity>();
                    userEntity.Data = Serialize(gameDataWithUserId.StoredGameData);
                    userEntity.UserId = gameDataWithUserId.UserId.ToString();

                    uow.AttachEntity(userEntity);
                }

                uow.Commit();
            }
        }

        public static string Serialize(StoredGameData obj)
        {
            return JsonConvert.SerializeObject(obj, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            });
        }

        public static StoredGameData Deserialize(string json)
        {
            return JsonConvert.DeserializeObject<StoredGameData>(json, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            });
        }
    }
}
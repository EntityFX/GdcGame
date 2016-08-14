using EntityFX.Gdcame.DataAccess.Contract.GameData;
using EntityFX.Gdcame.DataAccess.Model;
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

        public void CreateForUser(int userId, StoredGameData gameData)
        {

            using (var uow = _unitOfWorkFactory.Create())
            {
                var userEntity = uow.CreateEntity<UserGameDataSnapshotEntity>();
                userEntity.Data = Serialize(gameData);
                userEntity.UserId = userId;
                uow.Commit();
            }

        }

        public void UpdateForUser(int userId, StoredGameData gameData)
        {
            using (var uow = _unitOfWorkFactory.Create())
            {
                var userCounter = uow.CreateEntity<UserGameDataSnapshotEntity>();
                userCounter.UserId = userId;

                userCounter.Data = Serialize(gameData);

                uow.AttachEntity(userCounter);
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
namespace EntityFX.Gdcame.DataAccess.Repository.Ef.MainServer
{
    using EntityFX.Gdcame.DataAccess.Contract.MainServer.GameData.Store;
    using EntityFX.Gdcame.DataAccess.Repository.Contract.MainServer;
    using EntityFX.Gdcame.DataAccess.Repository.Contract.MainServer.Criterions.UserGameSnapshot;
    using EntityFX.Gdcame.DataAccess.Repository.Ef.MainServer.Entities;
    using EntityFX.Gdcame.Infrastructure.Repository.UnitOfWork;

    using Newtonsoft.Json;
    using System;

    public class UserGameSnapshotRepository : IUserGameSnapshotRepository
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        public UserGameSnapshotRepository(IUnitOfWorkFactory unitOfWorkFactory
            )
        {
            this._unitOfWorkFactory = unitOfWorkFactory;
        }

        public StoredGameData FindByUserId(GetUserGameSnapshotByIdCriterion criterion)
        {
            using (var uow = this._unitOfWorkFactory.Create())
            {
                var findQuery = uow.BuildQuery();
                var entity = findQuery.For<UserGameDataSnapshotEntity>()
                    .With(criterion);
                if (entity == null) return null;
                return Deserialize(entity.Data);
            }
        }

        public StoredGameDataWithUserId[] FindChunked(GetGameSnapshotsByOffsetCriterion criterion)
        {
            throw new NotImplementedException();
        }

        public void CreateUserGames(StoredGameDataWithUserId[] listOfGameDataWithUserId)
        {

            using (var uow = this._unitOfWorkFactory.Create())
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

        public void CreateOrUpdateUserGames(StoredGameDataWithUserId[] listOfGameDataWithUserId)
        {
            using (var uow = this._unitOfWorkFactory.Create())
            {
                foreach (var gameDataWithUserId in listOfGameDataWithUserId)
                {
                    var userEntity = uow.CreateEntity<UserGameDataSnapshotEntity>();
                    userEntity.Data = Serialize(gameDataWithUserId.StoredGameData);
                    userEntity.UserId = gameDataWithUserId.UserId;
                    userEntity.CreateDateTime = gameDataWithUserId.CreateDateTime;
                    userEntity.UpdateDateTime = gameDataWithUserId.UpdateDateTime;
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
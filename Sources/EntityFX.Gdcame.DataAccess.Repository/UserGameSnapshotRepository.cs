using System;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using EntityFX.Gdcame.Common.Contract;
using EntityFX.Gdcame.DataAccess.Model;
using EntityFX.Gdcame.DataAccess.Repository.Criterions.UserGameSnapshot;
using EntityFX.Gdcame.Infrastructure.Repository.UnitOfWork;
using EntityFX.Gdcame.DataAccess.Contract.GameData;
using Newtonsoft.Json;

namespace EntityFX.Gdcame.DataAccess.Repository
{
    public class UserGameSnapshotRepository : IUserGameSnapshotRepository
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        public UserGameSnapshotRepository(IUnitOfWorkFactory unitOfWorkFactory

            )
        {
            _unitOfWorkFactory = unitOfWorkFactory;

        }

        public StoreGameData FindByUserId(GetUserGameSnapshotByIdCriterion criterion)
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

        public void CreateForUser(int userId, StoreGameData gameData)
        {

            using (var uow = _unitOfWorkFactory.Create())
            {
                var userEntity = uow.CreateEntity<UserGameDataSnapshotEntity>();
                userEntity.Data = Serialize(gameData);
                userEntity.UserId = userId;
                uow.Commit();
            }

        }

        public void UpdateForUser(int userId, StoreGameData gameData)
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

        public static string Serialize(StoreGameData obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public static StoreGameData Deserialize(string json)
        {
            return JsonConvert.DeserializeObject<StoreGameData>(json);
        }
    }
}
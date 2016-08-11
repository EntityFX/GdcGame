using System;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using EntityFX.Gdcame.Common.Contract;
using EntityFX.Gdcame.DataAccess.Model;
using EntityFX.Gdcame.DataAccess.Repository.Criterions.UserGameSnapshot;
using EntityFX.Gdcame.Infrastructure.Repository.UnitOfWork;

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

        public GameData FindByUserId(GetUserGameSnapshotByIdCriterion criterion)
        {
            using (var uow = _unitOfWorkFactory.Create())
            {
                var findQuery = uow.BuildQuery();
                var entity = findQuery.For<UserGameDataSnapshotEntity>()
                    .With(criterion);
                if (entity == null) return null;
                return (GameData)Deserialize(entity.Data, typeof(GameData));
            }
        }

        public void CreateForUser(int userId, GameData gameData)
        {
            using (var uow = _unitOfWorkFactory.Create())
            {
                var userEntity = uow.CreateEntity<UserGameDataSnapshotEntity>();
                userEntity.Data = Serialize(gameData);
                userEntity.UserId = userId;
                uow.Commit();
            }

        }

        public void UpdateForUser(int userId, GameData gameData)
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

        public static string Serialize(object obj)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                DataContractSerializer serializer = new DataContractSerializer(obj.GetType());
                serializer.WriteObject(memoryStream, obj);
                return Encoding.UTF8.GetString(memoryStream.ToArray());
            }
        }

        public static object Deserialize(string xml, Type toType)
        {
            using (MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(xml)))
            {
                XmlDictionaryReader reader = XmlDictionaryReader.CreateTextReader(memoryStream, Encoding.UTF8, new XmlDictionaryReaderQuotas(), null);
                DataContractSerializer serializer = new DataContractSerializer(toType);
                return serializer.ReadObject(reader);
            }
        }
    }
}
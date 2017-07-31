namespace EntityFX.Gdcame.DataAccess.Repository.LocalStorage.Common
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using EntityFX.Gdcame.DataAccess.Contract.Common.User;
    using EntityFX.Gdcame.DataAccess.Repository.Contract.Common;
    using EntityFX.Gdcame.DataAccess.Repository.Contract.Common.Criterions.User;

    using Newtonsoft.Json;

    public class UserRepository : IUserRepository
    {
        public UserRepository()
        {
            if (!Directory.Exists(GetUserStorageFilePath()))
            {
                Directory.CreateDirectory(GetUserStorageFilePath());
            }
        }

        public int Create(User user)
        {
            using (StreamWriter file = File.CreateText(GetUserStorageFilePath(user.Id + ".json")))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, user);
            }
            using (StreamWriter file = File.CreateText(GetUserStorageFilePath(user.Login + ".index")))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, user.Id);
            }
            return 0;
        }

        public void Delete(string id)
        {
            var res = this.FindById(new GetUserByIdCriterion(id));
            if (res == null) return;

            if (!File.Exists(GetUserStorageFilePath(id + ".json")))
            {
                return;
            }
            File.Delete(GetUserStorageFilePath(id + ".json"));

            if (File.Exists(GetUserStorageFilePath(res.Login + ".index")))
            {
                File.Delete(GetUserStorageFilePath(res.Login + ".index"));
            }
        }

        public IEnumerable<User> FindAll(GetAllUsersCriterion finalAllCriterion)
        {
            var di = new DirectoryInfo(GetUserStorageFilePath()).GetFiles("*.json");
            return di.Select(_ =>
                this.FindById(new GetUserByIdCriterion(Path.GetFileNameWithoutExtension(_.Name)))
            ).ToArray();
        }

        public IEnumerable<User> FindByFilter(GetUsersBySearchStringCriterion findByIdCriterion)
        {
            var di = new DirectoryInfo(GetUserStorageFilePath()).GetFiles("*" + findByIdCriterion.SearchString + "*.json");
            return di.Select(_ =>
                this.FindById(new GetUserByIdCriterion(Path.GetFileNameWithoutExtension(_.Name)))
            ).ToArray();
        }

        public int Count()
        {
            var di = new DirectoryInfo(GetUserStorageFilePath()).GetFiles("*.json");
            return di.Length;
        }

        public User FindById(GetUserByIdCriterion findByIdCriterion)
        {
            if (!File.Exists(GetUserStorageFilePath(findByIdCriterion.Id + ".json")))
            {
                return null;
            }
            // deserialize JSON directly from a file
            using (StreamReader file = File.OpenText(GetUserStorageFilePath(findByIdCriterion.Id + ".json")))
            {
                JsonSerializer serializer = new JsonSerializer();
                return (User)serializer.Deserialize(file, typeof(User));
            }
        }

        public User FindByName(GetUserByNameCriterion findByIdCriterion)
        {
            if (!File.Exists(GetUserStorageFilePath(findByIdCriterion.Name + ".index")))
            {
                return null;
            }
            using (StreamReader file = File.OpenText(GetUserStorageFilePath(findByIdCriterion.Name + ".index")))
            {
                JsonSerializer serializer = new JsonSerializer();
                var userId = (string)serializer.Deserialize(file, typeof(string));

                using (StreamReader userFile = File.OpenText(GetUserStorageFilePath(userId + ".json")))
                {
                    JsonSerializer userSerializer = new JsonSerializer();
                    return (User)serializer.Deserialize(userFile, typeof(User));
                }
            }
        }

        public void Update(User user)
        {
            this.Create(user);
        }

        private static string GetUserStorageFilePath(string fileName = null)
        {
            return fileName != null ? Path.Combine("storage", "users", fileName) : Path.Combine("storage", "users");
        }

        public IEnumerable<User> FindChunked(GetUsersByOffsetCriterion offsetCriterion)
        {
            return
                new DirectoryInfo(GetUserStorageFilePath()).GetFiles("*.json")
                    .Skip(offsetCriterion.Offset)
                    .Take(offsetCriterion.Size)
                    .Select(
                        _ => this.FindById(new GetUserByIdCriterion(Path.GetFileNameWithoutExtension(_.Name))));
        }
    }
}

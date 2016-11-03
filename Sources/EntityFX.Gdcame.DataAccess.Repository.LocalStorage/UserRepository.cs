using EntityFX.Gdcame.DataAccess.Repository.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFX.Gdcame.DataAccess.Repository.Contract.Criterions.User;
using EntityFX.Gdcame.DataAccess.Contract.User;
using System.IO;
using Newtonsoft.Json;

namespace EntityFX.Gdcame.DataAccess.Repository.LocalStorage
{
    public class UserRepository : IUserRepository
    {
        public UserRepository()
        {
            if (!Directory.Exists(GetUserStorageFilePath()))
            {
                Directory.CreateDirectory(GetUserStorageFilePath());
            }
        }

        public int Create(DataAccess.Contract.User.User user)
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
            var res = FindById(new GetUserByIdCriterion(id));
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

        public DataAccess.Contract.User.User[] FindAll(GetAllUsersCriterion finalAllCriterion)
        {
            var di = new DirectoryInfo(GetUserStorageFilePath()).GetFiles("*.json");
            return di.Select(_ =>
                FindById(new GetUserByIdCriterion(Path.GetFileNameWithoutExtension(_.Name)))
            ).ToArray();
        }

        public DataAccess.Contract.User.User[] FindByFilter(GetUsersBySearchStringCriterion findByIdCriterion)
        {
            var di = new DirectoryInfo(GetUserStorageFilePath()).GetFiles("*" + findByIdCriterion.SearchString + "*.json");
            return di.Select(_ =>
                FindById(new GetUserByIdCriterion(Path.GetFileNameWithoutExtension(_.Name)))
            ).ToArray();
        }

        public int Count()
        {
            var di = new DirectoryInfo(GetUserStorageFilePath()).GetFiles("*.json");
            return di.Length;
        }

        public DataAccess.Contract.User.User FindById(GetUserByIdCriterion findByIdCriterion)
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

        public DataAccess.Contract.User.User FindByName(GetUserByNameCriterion findByIdCriterion)
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

        public void Update(DataAccess.Contract.User.User user)
        {
            Create(user);
        }

        private static string GetUserStorageFilePath(string fileName = null)
        {
            return fileName != null ? Path.Combine("storage", "users", fileName) : Path.Combine("storage", "users");
        }
    }
}

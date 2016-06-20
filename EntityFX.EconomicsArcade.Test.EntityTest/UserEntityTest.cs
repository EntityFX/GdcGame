using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EntityFX.EconomicsArcade.DataAccess.Model;

namespace EntityFX.EconomicsArcade.Test.EntityTest
{
    [TestClass]
    public class UserEntityTest
    {
        private readonly static string _connString = "Data Source=.;Initial Catalog=EntityFX.EconomicsArcade.Database;Integrated Security=True";
        
        [TestMethod]
        public void CreateUserEntity()
        {
            var entity = new UserEntity() {
                Email = "email@mail.com",
                CreateDateTime = DateTime.Now
            };
            EconomicsArcadeDbContext ctx = new EconomicsArcadeDbContext(_connString);
            ctx.UserEntitySet.Add(entity);
            ctx.SaveChanges();
            //ctx.UserEntitySet.
        }

        [TestMethod]
        public void UpdateUserEntity()
        {
            EconomicsArcadeDbContext ctx = new EconomicsArcadeDbContext(_connString);
            var userEntity = ctx.UserEntitySet.Find(1);
            userEntity.UpdateDateTime = DateTime.Now; 
            ctx.SaveChanges();
            //ctx.UserEntitySet.
        }
    }
}

﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EntityFX.EconomicsArcade.DataAccess.Model;

namespace EntityFX.EconomicsArcade.Test.EntityTest
{
    [TestClass]
    public class UserEntityTest
    {
        //private static readonly string ConnString = "Data Source=.;Initial Catalog=EntityFX.EconomicsArcade.Database;Integrated Security=True";
        private static readonly string ConnString = "Data Source=amukintern.testrussia.local;Initial Catalog=IclServicesWcfTestLab;Persist Security Info=True;User ID=sa;Password=P@ssword;MultipleActiveResultSets=True;App=EntityFramework";
        
        [TestMethod]
        public void CreateUserEntity()
        {
            var entity = new UserEntity() {
                Email = "email@mail.com",
                CreateDateTime = DateTime.Now
            };
            EconomicsArcadeDbContext ctx = new EconomicsArcadeDbContext(ConnString);
            ctx.UserEntitySet.Add(entity);
            ctx.SaveChanges();
            //ctx.UserEntitySet.
        }

        [TestMethod]
        public void UpdateUserEntity()
        {
            EconomicsArcadeDbContext ctx = new EconomicsArcadeDbContext(ConnString);
            var userEntity = ctx.UserEntitySet.Find(1);
            userEntity.UpdateDateTime = DateTime.Now; 
            ctx.SaveChanges();
            //ctx.UserEntitySet.
        }
    }
}

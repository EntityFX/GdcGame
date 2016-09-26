using EntityFX.Gdcame.Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using EntityFX.Gdcame.DataAccess.Repository.Contract;

namespace EntityFX.Gdcame.DataAccess.Repository.LocalStorage
{
    public class ContainerBootstrapper : IContainerBootstrapper
    {
        public IUnityContainer Configure(IUnityContainer container)
        {
            container.RegisterType<IUserRepository, UserRepository>();
            container.RegisterType<IItemRepository, ItemRepository>();
            container.RegisterType<ICountersRepository, CountersRepository>();
            container.RegisterType<IUserRatingRepository, UserRatingRepository>();
            container.RegisterType<ICustomRuleRepository, CustomRuleRepository>();
            container.RegisterType<IUserGameSnapshotRepository, UserGameSnapshotRepository>();
            return container;
        }
    }
}

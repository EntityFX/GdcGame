﻿using EntityFX.EconomicsArcade.Contract.Manager.SessionManager;
using EntityFX.EconomicsArcade.Infrastructure.Common;
using EntityFX.EconomicsArcade.Manager;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFX.EconomicsArcade.Contract.Game;
using EntityFX.EconomicsArcade.Contract.Manager.GameManager;
using EntityFX.EconomicsArcade.Contract.Manager.GameManager.Incrementors;
using EntityFX.EconomicsArcade.Manager.Mappers;

namespace EntityFX.EconomicsArcade.Utils.ServiceStarter.Manager
{
    public class ContainerBootstrapper : IContainerBootstrapper
    {
        //public 
        public IUnityContainer Configure(IUnityContainer container)
        {
            container.RegisterType<ISessionManager, SessionManager>();
            container.RegisterType<IMapper<IncrementorBase, Incrementor>, IncrementorContractMapper>();
            container.RegisterType<IMapper<CounterBase, Contract.Manager.GameManager.Counters.CounterBase>, CounterContractMapper>();
            container.RegisterType<IMapper<FundsCounters, Contract.Manager.GameManager.Counters.FundsCounters>, FundsCountersContractMapper>();
            container.RegisterType<IMapper<FundsDriver, Contract.Manager.GameManager.Funds.FundsDriver>, FundsDriverContractMapper>();
            container.RegisterType<IMapper<IGame, GameData>, GameDataContractMapper>();
            container.RegisterType<FundsCountersContractMapper>();
            container.RegisterType<IGameManager, GameManager>(); 
            return container;
        }
    }
}

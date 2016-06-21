﻿using EntityFX.EconomicsArcade.Contract.Common.Funds;
using EntityFX.EconomicsArcade.Contract.Common.Incrementors;
using EntityFX.EconomicsArcade.Contract.DataAccess.GameData;
using EntityFX.EconomicsArcade.DataAccess.Repository;
using EntityFX.EconomicsArcade.DataAccess.Repository.Criterions.FundsDriver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFX.EconomicsArcade.DataAccess.Service
{
    public class GameDataDataAccessService : IGameDataDataAccessService
    {
        private readonly IFundsDriverRepository _fundsDriverRepository;
        
        public GameDataDataAccessService(IFundsDriverRepository fundsDriverRepository)
        {
            _fundsDriverRepository = fundsDriverRepository;
        }
        
        public Contract.Common.GameData GetGameData()
        {
            var fundsDrivers = _fundsDriverRepository.FindAll(new GetAllFundsDriversCriterion());

            return new Contract.Common.GameData()
            {
                FundsDrivers = fundsDrivers
            };
        }
    }
}

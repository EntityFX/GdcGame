﻿using EntityFX.EconomicsArcade.Contract.Common.Funds;
using EntityFX.EconomicsArcade.Contract.Common.Incrementors;
using EntityFX.EconomicsArcade.Contract.DataAccess.GameData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFX.EconomicsArcade.DataAccess.Service
{
    public class GameDataRepository : IGameDataRepository
    {
        public Contract.Common.GameData GetGameData()
        {
            var fundDrivers = new FundsDriver[] {

                    new FundsDriver {
                        Id = 0,
                        Value = 200,
                        BuyCount = 0,
                        UnlockValue = 0,
                        InflationPercent  = 0,
                        Incrementors = new Dictionary<int, Incrementor> {
                            {
                                (int)1,
                                new Incrementor() {
                                    Value = 10, IncrementorType = IncrementorTypeEnum.ValueIncrementor
                                }
                            } ,
                            {
                                (int)0,
                                new Incrementor {
                                    Value = 1, IncrementorType = IncrementorTypeEnum.ValueIncrementor
                                }
                            } ,
                        },
                        Name = "Matches"
                    }   
                ,

                    new FundsDriver {
                                                Id = 1,
                        Value = 400,
                        UnlockValue = 5,
                        Incrementors = new Dictionary<int, Incrementor> {
                            {
                                2,
                                new Incrementor() {
                                    Value = 10, IncrementorType = IncrementorTypeEnum.ValueIncrementor

                                }
                            } ,
                            {
                                (int)0,
                                new Incrementor{
                                    Value = 1, IncrementorType = IncrementorTypeEnum.ValueIncrementor
                                }
                            } ,
                        },
                        Name = "Bubble gum"
                    }   
                };
            return new Contract.Common.GameData()
            {
                FundsDrivers = fundDrivers
            };
        }
    }
}

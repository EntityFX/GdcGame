using System;
using EntityFX.EconomicsArcade.Contract.Common.Funds;
using EntityFX.EconomicsArcade.DataAccess.Model;
using EntityFX.EconomicsArcade.Infrastructure.Common;

namespace EntityFX.EconomicsArcade.DataAccess.Repository.Mappers
{
    public class UserFundsDriverContractMapper : IMapper<UserFundsDriverEntity, FundsDriver>
    {

        public FundsDriver Map(UserFundsDriverEntity source, FundsDriver destination = null)
        {
            destination = destination ?? new FundsDriver();
            destination.BuyCount = source.BuyCount;
            destination.Value = source.Value;
            destination.Id = source.FundsDriverId;
            return destination;
        }
    }
}
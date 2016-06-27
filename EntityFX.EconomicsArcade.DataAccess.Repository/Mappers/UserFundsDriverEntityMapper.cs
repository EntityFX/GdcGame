using System;
using EntityFX.EconomicsArcade.Contract.Common.Funds;
using EntityFX.EconomicsArcade.DataAccess.Model;
using EntityFX.EconomicsArcade.Infrastructure.Common;

namespace EntityFX.EconomicsArcade.DataAccess.Repository.Mappers
{
    public class UserFundsDriverEntityMapper : IMapper<FundsDriver, UserFundsDriverEntity>
    {

        public UserFundsDriverEntity Map(FundsDriver source, UserFundsDriverEntity destination = null)
        {
            destination = destination ?? new UserFundsDriverEntity();
            destination.BuyCount = source.BuyCount;
            //destination.CreateDateTime = DateTime.Now;
            destination.FundsDriverId = source.Id;
            destination.Value = source.Value;
            return destination;
        }
    }
}
using EntityFX.Gdcame.Common.Contract.Funds;
using EntityFX.Gdcame.DataAccess.Model;
using EntityFX.Gdcame.Infrastructure.Common;

namespace EntityFX.Gdcame.DataAccess.Repository.Mappers
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
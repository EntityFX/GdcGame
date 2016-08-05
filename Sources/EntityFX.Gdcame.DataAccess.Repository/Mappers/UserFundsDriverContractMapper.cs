using EntityFX.Gdcame.Common.Contract.Funds;
using EntityFX.Gdcame.DataAccess.Model;
using EntityFX.Gdcame.Infrastructure.Common;

namespace EntityFX.Gdcame.DataAccess.Repository.Mappers
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
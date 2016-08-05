using EntityFX.Gdcame.Common.Contract.Funds;
using EntityFX.Gdcame.DataAccess.Model;
using EntityFX.Gdcame.Infrastructure.Common;

namespace EntityFX.Gdcame.DataAccess.Repository.Mappers
{
    public class UserCustomRuleEntityMapper : IMapper<CustomRuleInfo, UserCustomRuleEntity>
    {
        public UserCustomRuleEntity Map(CustomRuleInfo source, UserCustomRuleEntity destination = null)
        {
            destination = destination ?? new UserCustomRuleEntity();

            destination.CurrentIndex = source.CurrentIndex;
            destination.CustomRuleId = source.CustomRuleId;
            destination.FundsDriverId = source.FundsDriverId;
            return destination;
        }
    }
}
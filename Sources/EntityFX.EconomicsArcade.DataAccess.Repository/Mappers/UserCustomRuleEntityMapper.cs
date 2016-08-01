using EntityFX.EconomicsArcade.Contract.Common.Funds;
using EntityFX.EconomicsArcade.DataAccess.Model;
using EntityFX.EconomicsArcade.Infrastructure.Common;

namespace EntityFX.EconomicsArcade.DataAccess.Repository.Mappers
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
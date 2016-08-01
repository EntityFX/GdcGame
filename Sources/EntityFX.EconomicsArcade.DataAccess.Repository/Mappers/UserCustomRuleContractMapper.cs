using EntityFX.EconomicsArcade.Contract.Common.Funds;
using EntityFX.EconomicsArcade.DataAccess.Model;
using EntityFX.EconomicsArcade.Infrastructure.Common;

namespace EntityFX.EconomicsArcade.DataAccess.Repository.Mappers
{
    public class UserCustomRuleContractMapper : IMapper<UserCustomRuleEntity, CustomRuleInfo>
    {
        public CustomRuleInfo Map(UserCustomRuleEntity source, CustomRuleInfo destination = null)
        {
            return new CustomRuleInfo()
            {
                CurrentIndex = source.CurrentIndex,
                CustomRuleId = source.CustomRuleId,
                FundsDriverId = source.FundsDriverId
            };
        }
    }
}
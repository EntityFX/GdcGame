using EntityFX.Gdcame.Common.Contract.Funds;
using EntityFX.Gdcame.DataAccess.Model;
using EntityFX.Gdcame.Infrastructure.Common;

namespace EntityFX.Gdcame.DataAccess.Repository.Mappers
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
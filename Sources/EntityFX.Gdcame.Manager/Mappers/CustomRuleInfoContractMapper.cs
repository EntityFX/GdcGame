using EntityFX.Gdcame.GameEngine.Contract.Items;
using EntityFX.Gdcame.Infrastructure.Common;

namespace EntityFX.Gdcame.Manager.Mappers
{
    public class CustomRuleInfoContractMapper : IMapper<CustomRuleInfo, Common.Contract.Items.CustomRuleInfo>
    {

        public Common.Contract.Items.CustomRuleInfo Map(CustomRuleInfo source, Common.Contract.Items.CustomRuleInfo destination)
        {
            return new Common.Contract.Items.CustomRuleInfo()
            {
                CustomRuleId = source.CustomRule.Id,
                CurrentIndex = source.CurrentIndex
            };
        }
    }
}
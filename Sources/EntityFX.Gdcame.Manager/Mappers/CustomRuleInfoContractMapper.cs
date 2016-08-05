using EntityFX.Gdcame.GameEngine.Contract.Funds;
using EntityFX.Gdcame.Infrastructure.Common;

namespace EntityFX.Gdcame.Manager.Mappers
{
    public class CustomRuleInfoContractMapper : IMapper<CustomRuleInfo, Gdcame.Common.Contract.Funds.CustomRuleInfo>
    {

        public Gdcame.Common.Contract.Funds.CustomRuleInfo Map(CustomRuleInfo source, Gdcame.Common.Contract.Funds.CustomRuleInfo destination)
        {
            return new Gdcame.Common.Contract.Funds.CustomRuleInfo()
            {
                CustomRuleId = source.CustomRule.Id,
                CurrentIndex = source.CurrentIndex
            };
        }
    }
}
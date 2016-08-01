using System.Linq;
using EntityFX.EconomicsArcade.Contract.Common.Incrementors;
using EntityFX.EconomicsArcade.Contract.Game.Funds;
using EntityFX.EconomicsArcade.Contract.Game.Incrementors;
using EntityFX.EconomicsArcade.Infrastructure.Common;

namespace EntityFX.EconomicsArcade.Manager.Mappers
{
    public class CustomRuleInfoContractMapper : IMapper<CustomRuleInfo, Contract.Common.Funds.CustomRuleInfo>
    {

        public Contract.Common.Funds.CustomRuleInfo Map(CustomRuleInfo source, Contract.Common.Funds.CustomRuleInfo destination)
        {
            return new Contract.Common.Funds.CustomRuleInfo()
            {
                CustomRuleId = source.CustomRule.Id,
                CurrentIndex = source.CurrentIndex
            };
        }
    }
}
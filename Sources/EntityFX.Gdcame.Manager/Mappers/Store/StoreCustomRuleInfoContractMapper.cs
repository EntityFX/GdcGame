using EntityFX.Gdcame.GameEngine.Contract.Funds;
using EntityFX.Gdcame.Infrastructure.Common;

using EntityFX.Gdcame.DataAccess.Contract.GameData;

namespace EntityFX.Gdcame.Manager.Mappers
{
    public class StoreCustomRuleInfoContractMapper : IMapper<CustomRuleInfo, StoreCustomRuleInfo>
    {

        public StoreCustomRuleInfo Map(CustomRuleInfo source, StoreCustomRuleInfo destination)
        {
            return new StoreCustomRuleInfo()
            {
                CustomRuleId = source.CustomRule.Id,
                CurrentIndex = source.CurrentIndex
            };
        }
    }
}
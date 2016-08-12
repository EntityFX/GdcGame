using EntityFX.Gdcame.GameEngine.Contract.Items;
using EntityFX.Gdcame.Infrastructure.Common;

using EntityFX.Gdcame.DataAccess.Contract.GameData;

namespace EntityFX.Gdcame.Manager.Mappers
{
    public class StoreCustomRuleInfoContractMapper : IMapper<CustomRuleInfo, StoredCustomRuleInfo>
    {

        public StoredCustomRuleInfo Map(CustomRuleInfo source, StoredCustomRuleInfo destination)
        {
            return new StoredCustomRuleInfo()
            {
                Id = source.CustomRule.Id,
                CurrentIndex = source.CurrentIndex
            };
        }
    }
}
using EntityFX.Gdcame.DataAccess.Contract.GameData.Store;
using EntityFX.Gdcame.GameEngine.Contract.Items;
using EntityFX.Gdcame.Infrastructure.Common;

namespace EntityFX.Gdcame.Manager.MainServer.Mappers.Store
{
    public class StoreCustomRuleInfoContractMapper : IMapper<CustomRuleInfo, StoredCustomRuleInfo>
    {
        public StoredCustomRuleInfo Map(CustomRuleInfo source, StoredCustomRuleInfo destination)
        {
            return new StoredCustomRuleInfo
            {
                Id = source.CustomRule.Id,
                CurrentIndex = source.CurrentIndex
            };
        }
    }
}
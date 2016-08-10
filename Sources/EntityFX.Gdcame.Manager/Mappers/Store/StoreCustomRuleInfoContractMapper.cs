using EntityFX.Gdcame.GameEngine.Contract.Funds;
using EntityFX.Gdcame.Infrastructure.Common;

using EntityFX.Gdcame.DataAccess.Contract.GameData;

namespace EntityFX.Gdcame.Manager.Mappers
{
    public class StoreCustomRuleInfoContractMapper : IMapper<StoreCustomRuleInfo, Gdcame.Common.Contract.Funds.StoreCustomRuleInfo>
    {

        public Gdcame.Common.Contract.Funds.StoreCustomRuleInfo Map(StoreCustomRuleInfo source, Gdcame.Common.Contract.Funds.StoreCustomRuleInfo destination)
        {
            return new Gdcame.Common.Contract.Funds.StoreCustomRuleInfo()
            {
                CustomRuleId = source.CustomRule.Id,
                CurrentIndex = source.CurrentIndex
            };
        }
    }
}
using EntityFX.Gdcame.GameEngine.Contract.Items;
using EntityFX.Gdcame.Infrastructure.Common;

namespace EntityFX.Gdcame.Manager.MainServer.Mappers
{
    public class CustomRuleInfoContractMapper : IMapper<CustomRuleInfo, Gdcame.Common.Contract.Items.CustomRuleInfo>
    {
        public Gdcame.Common.Contract.Items.CustomRuleInfo Map(CustomRuleInfo source,
            Gdcame.Common.Contract.Items.CustomRuleInfo destination)
        {
            return new Gdcame.Common.Contract.Items.CustomRuleInfo
            {
                CustomRuleId = source.CustomRule.Id,
                CurrentIndex = source.CurrentIndex
            };
        }
    }
}
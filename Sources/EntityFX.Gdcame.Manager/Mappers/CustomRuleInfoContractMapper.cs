using EntityFX.Gdcame.Infrastructure.Common;

namespace EntityFX.Gdcame.Manager.MainServer.Mappers
{
    using EntityFX.Gdcame.Kernel.Contract.Items;

    public class CustomRuleInfoContractMapper : IMapper<CustomRuleInfo, Gdcame.Contract.MainServer.Items.CustomRuleInfo>
    {
        public Gdcame.Contract.MainServer.Items.CustomRuleInfo Map(CustomRuleInfo source,
            Gdcame.Contract.MainServer.Items.CustomRuleInfo destination)
        {
            return new Gdcame.Contract.MainServer.Items.CustomRuleInfo
            {
                CustomRuleId = source.CustomRule.Id,
                CurrentIndex = source.CurrentIndex
            };
        }
    }
}
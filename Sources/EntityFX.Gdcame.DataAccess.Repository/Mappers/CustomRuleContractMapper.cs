using EntityFX.Gdcame.Common.Contract.Items;
using EntityFX.Gdcame.DataAccess.Model;
using EntityFX.Gdcame.Infrastructure.Common;

namespace EntityFX.Gdcame.DataAccess.Repository.Ef.Mappers
{
    public class CustomRuleContractMapper : IMapper<CustomRuleEntity, CustomRule>
    {
        public CustomRule Map(CustomRuleEntity source, CustomRule destination = null)
        {
            destination = destination ?? new CustomRule();
            destination.Id = source.Id;
            destination.Name = source.Name;
            return destination;
        }
    }
}
using System.Collections.Generic;
using EntityFX.EconomicsArcade.Contract.Common.Funds;
using EntityFX.EconomicsArcade.Contract.Common.Incrementors;
using EntityFX.EconomicsArcade.DataAccess.Model;
using EntityFX.EconomicsArcade.Infrastructure.Common;

namespace EntityFX.EconomicsArcade.DataAccess.Repository.Mappers
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
using EntityFX.EconomicsArcade.Contract.Common.Funds;
using EntityFX.EconomicsArcade.DataAccess.Repository.Criterions.CustomRule;

namespace EntityFX.EconomicsArcade.DataAccess.Repository
{
    public interface ICustomRuleRepository
    {
        CustomRule[] FindAll(GetAllCustomRulesCriterion criterion); 
    }
}
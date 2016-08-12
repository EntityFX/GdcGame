using EntityFX.Gdcame.Common.Contract.Items;
using EntityFX.Gdcame.DataAccess.Repository.Criterions.CustomRule;

namespace EntityFX.Gdcame.DataAccess.Repository
{
    public interface ICustomRuleRepository
    {
        CustomRule[] FindAll(GetAllCustomRulesCriterion criterion); 
    }
}
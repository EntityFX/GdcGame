using EntityFX.Gdcame.Common.Contract.Items;
using EntityFX.Gdcame.DataAccess.Repository.Contract.Criterions.CustomRule;

namespace EntityFX.Gdcame.DataAccess.Repository.Contract
{
    public interface ICustomRuleRepository
    {
        CustomRule[] FindAll(GetAllCustomRulesCriterion criterion);
    }
}
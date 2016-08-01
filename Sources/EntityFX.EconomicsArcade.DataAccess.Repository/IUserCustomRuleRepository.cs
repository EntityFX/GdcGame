using EntityFX.EconomicsArcade.Contract.Common.Funds;
using EntityFX.EconomicsArcade.DataAccess.Repository.Criterions.UserCounter;
using EntityFX.EconomicsArcade.DataAccess.Repository.Queries.UserCustomRuleInfo;

namespace EntityFX.EconomicsArcade.DataAccess.Repository
{
    public interface IUserCustomRuleRepository
    {
        CustomRuleInfo[] FindByUserId(GetUserCustomRuleInfoByUserIdCriterion criterion);
        void CreateForUser(int userId, CustomRuleInfo[] fundsDrivers);
        void UpdateForUser(int userId, CustomRuleInfo[] fundsDrivers); 
    }
}
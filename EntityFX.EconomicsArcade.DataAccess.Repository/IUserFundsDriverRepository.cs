using EntityFX.EconomicsArcade.Contract.Common.Funds;
using EntityFX.EconomicsArcade.DataAccess.Repository.Criterions.UserFundsDriver;

namespace EntityFX.EconomicsArcade.DataAccess.Repository
{
    public interface IUserFundsDriverRepository
    {
        FundsDriver[] FindByUserId(GetUserFundsDriverByUserIdCriterion criterion);
        void CreateForUser(int userId, FundsDriver[] fundsDrivers);
        void UpdateForUser(int userId, FundsDriver[] fundsDrivers);
    }
}
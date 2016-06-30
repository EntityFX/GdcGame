using EntityFX.EconomicsArcade.Contract.Common.Funds;
using EntityFX.EconomicsArcade.DataAccess.Repository.Criterions.FundsDriver;

namespace EntityFX.EconomicsArcade.DataAccess.Repository
{
    public interface IFundsDriverRepository
    {
        FundsDriver[] FindAll(GetAllFundsDriversCriterion criterion);
    }
}
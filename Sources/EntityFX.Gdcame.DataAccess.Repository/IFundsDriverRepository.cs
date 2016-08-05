using EntityFX.Gdcame.Common.Contract.Funds;
using EntityFX.Gdcame.DataAccess.Repository.Criterions.FundsDriver;

namespace EntityFX.Gdcame.DataAccess.Repository
{
    public interface IFundsDriverRepository
    {
        FundsDriver[] FindAll(GetAllFundsDriversCriterion criterion);
    }
}
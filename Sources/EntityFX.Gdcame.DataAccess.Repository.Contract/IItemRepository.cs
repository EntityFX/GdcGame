using EntityFX.Gdcame.Common.Contract.Items;
using EntityFX.Gdcame.DataAccess.Repository.Contract.Criterions.FundsDriver;

namespace EntityFX.Gdcame.DataAccess.Repository.Contract
{
    public interface IItemRepository
    {
        Item[] FindAll(GetAllFundsDriversCriterion criterion);
    }
}
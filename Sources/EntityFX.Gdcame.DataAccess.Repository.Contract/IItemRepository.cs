namespace EntityFX.Gdcame.DataAccess.Repository.Contract.MainServer
{
    using EntityFX.Gdcame.Contract.MainServer.Items;
    using EntityFX.Gdcame.DataAccess.Repository.Contract.MainServer.Criterions.FundsDriver;

    public interface IItemRepository
    {
        Item[] FindAll(GetAllFundsDriversCriterion criterion);
    }
}
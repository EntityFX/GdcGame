using EntityFX.Gdcame.DataAccess.Contract.GameData.Store;
using EntityFX.Gdcame.GameEngine.Contract.Incrementors;
using EntityFX.Gdcame.Infrastructure.Common;

namespace EntityFX.Gdcame.Manager.MainServer.Mappers.Store
{
    public class StoreIncrementorContractMapper : IMapper<IncrementorBase, StoredIncrementor>
    {
        public StoredIncrementor Map(IncrementorBase source, StoredIncrementor destination)
        {
            return new StoredIncrementor
            {
                Value = source.Value
            };
        }
    }
}
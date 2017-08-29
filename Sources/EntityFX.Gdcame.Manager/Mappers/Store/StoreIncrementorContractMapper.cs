using EntityFX.Gdcame.Contract.MainServer.Store;
using EntityFX.Gdcame.Infrastructure.Common;

namespace EntityFX.Gdcame.Manager.MainServer.Mappers.Store
{
    using EntityFX.Gdcame.Kernel.Contract.Incrementors;

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
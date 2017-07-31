using EntityFX.Gdcame.Infrastructure.Common;

namespace EntityFX.Gdcame.EngineTestApplication
{
    using EntityFX.Gdcame.Contract.MainServer.Incrementors;
    using EntityFX.Gdcame.Kernel.Contract.Incrementors;

    using IncrementorTypeEnum = Gdcame.Contract.MainServer.Incrementors.IncrementorTypeEnum;

    public class IncrementorContractMapper : IMapper<IncrementorBase, Incrementor>
    {
        public Incrementor Map(IncrementorBase source, Incrementor destination)
        {
            return new Incrementor
            {
                IncrementorType = (IncrementorTypeEnum) ((int) source.IncrementorType),
                Value = source.Value
            };
        }
    }
}
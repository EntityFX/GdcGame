using EntityFX.Gdcame.Common.Contract.Incrementors;
using EntityFX.Gdcame.GameEngine.Contract.Incrementors;
using EntityFX.Gdcame.Infrastructure.Common;
using IncrementorTypeEnum = EntityFX.Gdcame.Common.Contract.Incrementors.IncrementorTypeEnum;

namespace EntityFx.Gdcame.TestApplication
{
    public class IncrementorContractMapper : IMapper<IncrementorBase, Incrementor>
    {
        public Incrementor Map(IncrementorBase source, Incrementor destination)
        {
            return new Incrementor()
            {
                IncrementorType = (IncrementorTypeEnum) ((int) source.IncrementorType),
                Value = source.Value
            };
        }
    }
}
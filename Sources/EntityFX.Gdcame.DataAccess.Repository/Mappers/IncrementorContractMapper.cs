namespace EntityFX.Gdcame.DataAccess.Repository.Ef.MainServer.Mappers
{
    using System;

    using EntityFX.Gdcame.Common.Contract.Incrementors;
    using EntityFX.Gdcame.DataAccess.Repository.Ef.MainServer.Entities;
    using EntityFX.Gdcame.Infrastructure.Common;

    public class IncrementorContractMapper : IMapper<IncrementorEntity, Incrementor>
    {
        public Incrementor Map(IncrementorEntity source, Incrementor destination = null)
        {
            destination = destination ?? new Incrementor();
            destination.IncrementorType = (IncrementorTypeEnum) source.Type;
            destination.Value = Convert.ToInt32(source.Value);
            return destination;
        }
    }
}
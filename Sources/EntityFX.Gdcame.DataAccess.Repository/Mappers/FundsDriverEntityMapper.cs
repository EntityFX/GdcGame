namespace EntityFX.Gdcame.DataAccess.Repository.Ef.MainServer.Mappers
{
    using System.Collections.Generic;

    using EntityFX.Gdcame.Contract.MainServer.Incrementors;
    using EntityFX.Gdcame.Contract.MainServer.Items;
    using EntityFX.Gdcame.DataAccess.Repository.Ef.MainServer.Entities;
    using EntityFX.Gdcame.Infrastructure.Common;

    public class FundsDriverEntityMapper : IMapper<Item, FundsDriverEntity>
    {
        private readonly IMapper<Incrementor, IncrementorEntity> _incrementorContractMapper;

        public FundsDriverEntityMapper(IMapper<Incrementor, IncrementorEntity> incrementorContractMapper)
        {
            this._incrementorContractMapper = incrementorContractMapper;
        }

        public FundsDriverEntity Map(Item source, FundsDriverEntity destination = null)
        {
            destination = destination ?? new FundsDriverEntity();
            destination.InitialValue = source.Price;
            destination.UnlockValue = source.UnlockValue;
            destination.Name = source.Name;
            destination.Id = source.Id;
            destination.InflationPercent = (short) source.InflationPercent;
            destination.Incrementors = new List<IncrementorEntity>();
            destination.Picture = source.Picture;
            foreach (var incrementor in source.Incrementors)
            {
                destination.Incrementors.Add(this._incrementorContractMapper.Map(incrementor));
            }
            return destination;
        }
    }
}
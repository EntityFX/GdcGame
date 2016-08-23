using System.Collections.Generic;
using EntityFX.Gdcame.Common.Contract.Incrementors;
using EntityFX.Gdcame.Common.Contract.Items;
using EntityFX.Gdcame.DataAccess.Model.Ef;
using EntityFX.Gdcame.Infrastructure.Common;

namespace EntityFX.Gdcame.DataAccess.Repository.Ef.Mappers
{
    public class FundsDriverEntityMapper : IMapper<Item, FundsDriverEntity>
    {
        private readonly IMapper<Incrementor, IncrementorEntity> _incrementorContractMapper;

        public FundsDriverEntityMapper(IMapper<Incrementor, IncrementorEntity> incrementorContractMapper)
        {
            _incrementorContractMapper = incrementorContractMapper;
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
                destination.Incrementors.Add(_incrementorContractMapper.Map(incrementor.Value));
            }
            return destination;
        }
    }
}
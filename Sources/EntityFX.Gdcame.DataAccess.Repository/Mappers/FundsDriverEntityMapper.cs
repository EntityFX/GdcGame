using System.Collections.Generic;
using EntityFX.Gdcame.Common.Contract.Funds;
using EntityFX.Gdcame.Common.Contract.Incrementors;
using EntityFX.Gdcame.DataAccess.Model;
using EntityFX.Gdcame.Infrastructure.Common;

namespace EntityFX.Gdcame.DataAccess.Repository.Mappers
{
    public class FundsDriverEntityMapper : IMapper<FundsDriver, FundsDriverEntity>
    {
        private readonly IMapper<Incrementor, IncrementorEntity> _incrementorContractMapper;

        public FundsDriverEntityMapper(IMapper<Incrementor, IncrementorEntity> incrementorContractMapper)
        {
            _incrementorContractMapper = incrementorContractMapper;
        }

        public FundsDriverEntity Map(FundsDriver source, FundsDriverEntity destination = null)
        {
            destination = destination ?? new FundsDriverEntity();
            destination.InitialValue = source.Value;
            destination.UnlockValue = source.UnlockValue;
            destination.Name = source.Name;
            destination.Id = source.Id;
            destination.InflationPercent = (short)source.InflationPercent;
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
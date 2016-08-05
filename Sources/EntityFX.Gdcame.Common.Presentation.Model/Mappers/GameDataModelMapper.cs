using System.Collections.Generic;
using EntityFX.Gdcame.Common.Contract;
using EntityFX.Gdcame.Common.Contract.Counters;
using EntityFX.Gdcame.Common.Contract.Funds;
using EntityFX.Gdcame.Infrastructure.Common;

namespace EntityFX.Gdcame.Common.Presentation.Model.Mappers
{
    public class GameDataModelMapper : IMapper<GameData, GameDataModel>
    {
        private readonly IMapper<FundsDriver, FundsDriverModel> _fundsDriverModelMapper;
        private readonly IMapper<FundsCounters, FundsCounterModel> _fundsCounterModelMapper;

        public GameDataModelMapper(
            IMapper<FundsDriver, FundsDriverModel> fundsDriverModelMapper
            , IMapper<FundsCounters, FundsCounterModel> fundsCounterModelMapper)
        {
            _fundsDriverModelMapper = fundsDriverModelMapper;
            _fundsCounterModelMapper = fundsCounterModelMapper;
        }

        public GameDataModel Map(GameData source, GameDataModel destination = null)
        {
            destination = destination ?? new GameDataModel();
            var fundsDriverModels = new List<FundsDriverModel>();
            foreach (var fundsDriver in source.FundsDrivers)
            {
                fundsDriverModels.Add(_fundsDriverModelMapper.Map(fundsDriver));
            }
            destination.FundsDrivers = fundsDriverModels.ToArray();
            destination.Counters = _fundsCounterModelMapper.Map(source.Counters);

            return destination;
        }
    }
}

﻿using System.Collections.Generic;
using EntityFX.Gdcame.Common.Application.Model;
using EntityFX.Gdcame.Infrastructure.Common;

namespace EntityFX.Gdcame.Application.Api.MainServer.Mappers
{
    using EntityFX.Gdcame.Contract.MainServer;
    using EntityFX.Gdcame.Contract.MainServer.Counters;
    using EntityFX.Gdcame.Contract.MainServer.Items;

    public class GameDataModelMapper : IMapper<GameData, GameDataModel>
    {
        private readonly IMapper<Cash, CashModel> _fundsCounterModelMapper;
        private readonly IMapper<Item, ItemModel> _fundsDriverModelMapper;

        public GameDataModelMapper(
            IMapper<Item, ItemModel> fundsDriverModelMapper
            , IMapper<Cash, CashModel> fundsCounterModelMapper)
        {
            _fundsDriverModelMapper = fundsDriverModelMapper;
            _fundsCounterModelMapper = fundsCounterModelMapper;
        }

        public GameDataModel Map(GameData source, GameDataModel destination = null)
        {
            destination = destination ?? new GameDataModel();
            var fundsDriverModels = new List<ItemModel>();
            foreach (var fundsDriver in source.Items)
            {
                fundsDriverModels.Add(_fundsDriverModelMapper.Map(fundsDriver));
            }
            destination.Items = fundsDriverModels.ToArray();
            destination.Cash = _fundsCounterModelMapper.Map(source.Cash);

            return destination;
        }
    }
}
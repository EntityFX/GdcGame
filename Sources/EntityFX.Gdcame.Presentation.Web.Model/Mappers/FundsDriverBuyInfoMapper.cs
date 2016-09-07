﻿using EntityFX.Gdcame.Common.Contract.Counters;
using EntityFX.Gdcame.Common.Contract.Items;
using EntityFX.Gdcame.Common.Presentation.Model;
using EntityFX.Gdcame.Presentation.Contract.Model;
using EntityFX.Gdcame.Infrastructure.Common;
using EntityFX.Gdcame.Manager.Contract.GameManager;

namespace EntityFX.Gdcame.Presentation.Web.Model.Mappers
{
    public class FundsDriverBuyInfoMapper : IMapper<BuyFundDriverResult, BuyItemModel>
    {
        private readonly IMapper<Cash, CashModel> _countersMapper;
        private readonly IMapper<Item, ItemModel> _fundsDriverMapper;

        public FundsDriverBuyInfoMapper(IMapper<Cash, CashModel> countersMapper,
            IMapper<Item, ItemModel> fundsDriverMapper)
        {
            _countersMapper = countersMapper;
            _fundsDriverMapper = fundsDriverMapper;
        }

        public BuyItemModel Map(BuyFundDriverResult source, BuyItemModel destination = null)
        {
            destination = destination ?? new BuyItemModel();
            destination.ModifiedCountersInfo = _countersMapper.Map(source.ModifiedCash);
            destination.ItemBuyInfo = _fundsDriverMapper.Map(source.ModifiedItem);
            return destination;
        }
    }
}
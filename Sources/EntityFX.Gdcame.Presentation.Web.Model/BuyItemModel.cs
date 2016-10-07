using EntityFX.Gdcame.Common.Application.Model;

namespace EntityFX.Gdcame.Application.Contract.Model
{
    public class BuyItemModel
    {
        public ItemModel ItemBuyInfo { get; set; }

        public CashModel ModifiedCountersInfo { get; set; }
    }
}
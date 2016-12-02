using System.Collections.Generic;
using System.Linq;
using EntityFX.Gdcame.Common.Application.Model;
using EntityFX.Gdcame.Common.Contract.Items;
using EntityFX.Gdcame.Infrastructure.Common;

namespace EntityFX.Gdcame.Application.WebApi.Mappers
{
    public class FundsDriverModelMapper : IMapper<Item, ItemModel>
    {
        public ItemModel Map(Item source, ItemModel destination = null)
        {
            destination = destination ?? new ItemModel();
            destination.Price = source.Price;
            destination.Bought = source.Bought;
            destination.Id = source.Id;
            destination.Incrementors = source.Incrementors.Select(
                (pair, i) => new KeyValuePair<int, string>(i, pair.Value.ToString()))
                .ToDictionary(pair => pair.Key, pair => pair.Value);
            destination.InflationPercent = source.InflationPercent;
            destination.Name = source.Name;
            destination.UnlockBalance = source.UnlockValue;
            destination.IsUnlocked = source.IsUnlocked;
            destination.Picture = source.Picture;
            return destination;
        }
    }
}
using System.Linq;
using EntityFX.Gdcame.Common.Application.Model;
using EntityFX.Gdcame.Infrastructure.Common;

namespace EntityFX.Presentation.Shared.KernelConsole
{

    using EntityFX.Gdcame.Kernel.Contract.Incrementors;
    using EntityFX.Gdcame.Kernel.Contract.Items;
    using System.Collections.Generic;

    public class FundsDriverContractMapper : IMapper<Item, ItemModel>
    {


        public ItemModel Map(Item source,
            ItemModel destination)
        {
            var destinationIncrementors = new Dictionary<int, string>();
                //source.Incrementors.ToDictionary(key => (int)key.IncrementorType, value => value.Value.ToString());
                var inc = 0;
            foreach (var incrementor in source.Incrementors)
            {
                destinationIncrementors.Add(inc, incrementor.Value.ToString());
                ++inc;
            }
            return new ItemModel
            {
                Id = source.Id,
                Bought = source.Bought,
                Incrementors = destinationIncrementors,
                InflationPercent = source.InflationPercent,
                Name = source.Name,
                UnlockBalance = source.UnlockBalance,
                Price = source.Price
            };
        }
    }

}
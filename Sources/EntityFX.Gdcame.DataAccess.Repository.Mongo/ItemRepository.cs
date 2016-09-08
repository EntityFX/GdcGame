using EntityFX.Gdcame.DataAccess.Repository.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFX.Gdcame.Common.Contract.Items;
using EntityFX.Gdcame.DataAccess.Repository.Contract.Criterions.FundsDriver;
using EntityFX.Gdcame.Common.Contract.Incrementors;

namespace EntityFX.Gdcame.DataAccess.Repository.Mongo
{
    public class ItemRepository : IItemRepository
    {
        public Item[] FindAll(GetAllFundsDriversCriterion criterion)
        {
            return new Item[] {
                new Item() {
                    Id = 1, Name = "Item", Price = 300, IsUnlocked = true, InitialValue = 100
                        , Incrementors = new Dictionary<int, Incrementor>() {
                            { 0, new Incrementor() { Value = 1, IncrementorType = IncrementorTypeEnum.ValueIncrementor } },
                            { 1, new Incrementor() { Value = 10, IncrementorType = IncrementorTypeEnum.ValueIncrementor } },
                            { 2, new Incrementor() { Value = 0, IncrementorType = IncrementorTypeEnum.ValueIncrementor } }
                         }
                    }
            };
        }
    }
}

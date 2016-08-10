using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EntityFX.Gdcame.DataAccess.Contract.GameData
{
    public class StoreFundsDriver
    {
        public int Id { get; set; }

        public decimal Value { get; set; }

        public int BuyCount { get; set; }

        public StoreCustomRuleInfo CustomRuleInfo { get; set; }

        public IDictionary<int, StoreIncrementor> Incrementors { get; set; }
    }
}
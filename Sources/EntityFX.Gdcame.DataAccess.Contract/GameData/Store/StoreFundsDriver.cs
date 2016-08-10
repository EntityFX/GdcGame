using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EntityFX.Gdcame.DataAccess.Contract.GameData
{
    public class StoreFundsDriver
    {
        public int Id { get; set; }

        public decimal Value { get; set; }

        public int BuyCount { get; set; }

        public StoreCustomRuleInfo CustomRule { get; set; }

        public StoreIncrementor[] Incrementors { get; set; }
    }
}
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EntityFX.Gdcame.DataAccess.Contract.GameData
{
    public class StoredItem
    {
        public int Id { get; set; }

        public decimal Value { get; set; }

        public int BuyCount { get; set; }

        public StoredCustomRuleInfo CustomRule { get; set; }

        public Dictionary<int, int> Incrementors { get; set; }
    }
}
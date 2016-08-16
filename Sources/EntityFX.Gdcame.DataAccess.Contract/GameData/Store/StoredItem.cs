using System.Collections.Generic;

namespace EntityFX.Gdcame.DataAccess.Contract.GameData.Store
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
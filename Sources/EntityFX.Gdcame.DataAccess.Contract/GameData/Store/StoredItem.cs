using System.Collections.Generic;

namespace EntityFX.Gdcame.DataAccess.Contract.GameData.Store
{
    public class StoredItem
    {
        public int Id { get; set; }

        public decimal Price { get; set; }

        public int Bought { get; set; }

        public StoredCustomRuleInfo CustomRule { get; set; }

        public Dictionary<int, int> Incrementors { get; set; }
    }
}
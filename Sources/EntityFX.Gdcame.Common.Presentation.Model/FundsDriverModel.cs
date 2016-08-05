using System.Collections.Generic;
using EntityFX.Gdcame.Common.Contract.Incrementors;

namespace EntityFX.Gdcame.Common.Presentation.Model
{
    public class FundsDriverModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Value { get; set; }
        public int InflationPercent { get; set; }
        public decimal UnlockValue { get; set; }
        public bool IsActive { get; set; }
        public int BuyCount { get; set; }
        public string Picture { get; set; }
        public IDictionary<int, Incrementor> Incrementors { get; set; } 
    }
}

﻿namespace EntityFX.Gdcame.Kernel.Contract.Items
{
    using EntityFX.Gdcame.Kernel.Contract.Incrementors;

    public class Item
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public decimal InitialPrice { get; set; }

        public int InflationPercent { get; set; }

        public decimal UnlockBalance { get; set; }

        public int Bought { get; set; }

        public CustomRuleInfo CustomRuleInfo { get; set; }

        public IncrementorBase[] Incrementors { get; set; }
    }
}
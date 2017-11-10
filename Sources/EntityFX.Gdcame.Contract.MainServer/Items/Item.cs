namespace EntityFX.Gdcame.Contract.MainServer.Items
{
    

    using EntityFX.Gdcame.Contract.MainServer.Incrementors;

    
    public class Item
    {
        
        public int Id { get; set; }

        
        public string Name { get; set; }

        
        public decimal InitialValue { get; set; }

        
        public decimal Price { get; set; }

        
        public int InflationPercent { get; set; }

        
        public int InflationSteps { get; set; }

        
        public decimal UnlockValue { get; set; }

        
        public int Bought { get; set; }

        
        public bool IsUnlocked { get; set; }

        
        public string Picture { get; set; }

        
        public CustomRuleInfo CustomRuleInfo { get; set; }

        
        public Incrementor[] Incrementors { get; set; }
    }
}
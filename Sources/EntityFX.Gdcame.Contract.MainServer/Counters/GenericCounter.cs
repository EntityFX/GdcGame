namespace EntityFX.Gdcame.Contract.MainServer.Counters
{
    

    
    public class GenericCounter : CounterBase
    {
        
        public decimal Bonus { get; set; }

        
        public int BonusPercentage { get; set; }

        
        public int Inflation { get; set; }

        
        public bool UseInAutoSteps { get; set; }

        
        public decimal SubValue { get; set; }

        
        public int CurrentSteps { get; set; }

        
        public int InflationIncreaseSteps { get; set; }
    }
}
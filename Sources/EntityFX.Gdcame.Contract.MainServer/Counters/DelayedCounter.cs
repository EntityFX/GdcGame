namespace EntityFX.Gdcame.Contract.MainServer.Counters
{
    

    
    public class DelayedCounter : CounterBase
    {
        
        public decimal UnlockValue { get; set; }

        
        public int MiningTimeSeconds { get; set; }

        
        public int SecondsRemaining { get; set; }
    }
}
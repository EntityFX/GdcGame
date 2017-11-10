namespace EntityFX.Gdcame.Contract.MainServer.Store
{
    

    
    public class StoredDelayedCounter : StoredCounterBase
    {
        
        public decimal DelayedValue { get; set; }

        
        public int SecondsRemaining { get; set; }
    }
}
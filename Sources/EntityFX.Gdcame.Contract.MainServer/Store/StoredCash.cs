namespace EntityFX.Gdcame.Contract.MainServer.Store
{
    

    
    public class StoredCash
    {
        
        public decimal Balance { get; set; }

        
        public decimal TotalEarned { get; set; }

        
        public StoredCounterBase[] Counters { get; set; }
    }
}
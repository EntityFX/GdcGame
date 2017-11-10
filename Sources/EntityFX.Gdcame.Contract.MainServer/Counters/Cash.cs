namespace EntityFX.Gdcame.Contract.MainServer.Counters
{
    

    
    public class Cash
    {
        
        public decimal OnHand { get; set; }

        
        public decimal Total { get; set; }

        
        public CounterBase[] Counters { get; set; }
    }
}
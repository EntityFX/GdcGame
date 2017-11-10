namespace EntityFX.Gdcame.Contract.MainServer.Store
{
    

    
    public class StoredGenericCounter : StoredCounterBase
    {
        
        public int BonusPercent { get; set; }

        
        public int Inflation { get; set; }

        
        public int CurrentSteps { get; set; }
    }
}
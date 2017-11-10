namespace EntityFX.Gdcame.Contract.MainServer.Store
{
    

    
    public class StoredGameData
    {
        
        public StoredItem[] Items { get; set; }

        
        public StoredCash Cash { get; set; }

        
        public int ManualStepsCount { get; set; }

        
        public int AutomatedStepsCount { get; set; }
    }
}
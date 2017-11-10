namespace EntityFX.Gdcame.Contract.MainServer
{
    

    using EntityFX.Gdcame.Contract.MainServer.Counters;
    using EntityFX.Gdcame.Contract.MainServer.Items;

    
    public class GameData
    {
        
        public Item[] Items { get; set; }

        
        public Cash Cash { get; set; }

        
        public CustomRule[] CustomRules { get; set; }

        
        public int ManualStepsCount { get; set; }

        
        public int AutomatedStepsCount { get; set; }
    }
}
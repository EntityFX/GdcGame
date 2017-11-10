namespace EntityFX.Gdcame.Contract.MainServer.Store
{
    using System;
    

    
    public class StoredGameDataWithUserId
    {
        
        public StoredGameData StoredGameData { get; set; }
        
        public string UserId { get; set; }
        
        public DateTime CreateDateTime { get; set; }
        
        public DateTime? UpdateDateTime { get; set; }
    }
}
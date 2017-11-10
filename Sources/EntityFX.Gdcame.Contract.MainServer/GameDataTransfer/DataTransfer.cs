using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using System.Threading.Tasks;
using EntityFX.Gdcame.Contract.MainServer.Store;

namespace EntityFX.Gdcame.Contract.MainServer.GameDataTransfer
{
    
    public class DataTransfer
    {
        
        public StoredGameDataWithUserId GameData { get; set; }
        
        public UserDataTransfer UserData { get; set; }
    }
}

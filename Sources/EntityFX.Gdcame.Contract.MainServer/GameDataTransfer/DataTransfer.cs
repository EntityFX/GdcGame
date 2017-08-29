using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using EntityFX.Gdcame.Contract.MainServer.Store;

namespace EntityFX.Gdcame.Contract.MainServer.GameDataTransfer
{
    [DataContract]
    public class DataTransfer
    {
        [DataMember]
        public StoredGameDataWithUserId GameData { get; set; }
        [DataMember]
        public UserDataTransfer UserData { get; set; }
    }
}

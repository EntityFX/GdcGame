using System.Runtime.Serialization;

namespace EntityFX.Gdcame.DataAccess.Contract.GameData
{
    [DataContract]
    public class StoreCustomRule
    {
        [DataMember]
        public int Id { get; set; }
    }
}
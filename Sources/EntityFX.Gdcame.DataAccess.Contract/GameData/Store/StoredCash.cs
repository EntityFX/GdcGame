using System.Runtime.Serialization;

namespace EntityFX.Gdcame.DataAccess.Contract.GameData.Store
{
    [DataContract]
    public class StoredCash
    {
        [DataMember]
        public decimal Balance { get; set; }

        [DataMember]
        public decimal TotalEarned { get; set; }

        [DataMember]
        public StoredCounterBase[] Counters { get; set; }
    }
}
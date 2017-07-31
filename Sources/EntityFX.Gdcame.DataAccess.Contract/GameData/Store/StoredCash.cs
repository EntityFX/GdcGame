namespace EntityFX.Gdcame.DataAccess.Contract.MainServer.GameData.Store
{
    using System.Runtime.Serialization;

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
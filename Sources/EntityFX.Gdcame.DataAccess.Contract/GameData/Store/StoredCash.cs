using System.Runtime.Serialization;
using EntityFX.Gdcame.Common.Contract.Counters;

namespace EntityFX.Gdcame.DataAccess.Contract.GameData
{
    [DataContract]
    public class StoredCash
    {
        [DataMember]
        public decimal CashOnHand { get; set; }
        [DataMember]
        public decimal TotalEarned { get; set; }
        [DataMember]
        public StoredCounterBase[] Counters { get; set; }
    }
}
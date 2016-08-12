using System.Runtime.Serialization;
using EntityFX.Gdcame.Common.Contract.Counters;
using EntityFX.Gdcame.Common.Contract.Items;

namespace EntityFX.Gdcame.Common.Contract
{
    [DataContract]
    public class GameData
    {
        [DataMember]
        public Item[] Items { get; set; }
        [DataMember]
        public Cash Cash { get; set; }
        [DataMember]
        public CustomRule[] CustomRules { get; set; }
        [DataMember]
        public int ManualStepsCount { get; set; }
        [DataMember]
        public int AutomatedStepsCount { get; set; }
    }
}
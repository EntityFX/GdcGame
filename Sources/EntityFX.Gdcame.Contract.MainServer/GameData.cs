namespace EntityFX.Gdcame.Contract.MainServer
{
    using System.Runtime.Serialization;

    using EntityFX.Gdcame.Contract.MainServer.Counters;
    using EntityFX.Gdcame.Contract.MainServer.Items;

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
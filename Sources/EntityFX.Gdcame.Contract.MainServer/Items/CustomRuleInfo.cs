namespace EntityFX.Gdcame.Contract.MainServer.Items
{
    using System.Runtime.Serialization;

    [DataContract]
    public class CustomRuleInfo
    {
        [DataMember]
        public int CustomRuleId { get; set; }

        [DataMember]
        public int FundsDriverId { get; set; }

        [DataMember]
        public int? CurrentIndex { get; set; }
    }
}
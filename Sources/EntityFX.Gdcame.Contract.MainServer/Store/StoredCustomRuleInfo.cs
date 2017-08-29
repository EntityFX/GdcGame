namespace EntityFX.Gdcame.Contract.MainServer.Store
{
    using System.Runtime.Serialization;

    [DataContract]
    public class StoredCustomRuleInfo
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int? CurrentIndex { get; set; }
    }
}
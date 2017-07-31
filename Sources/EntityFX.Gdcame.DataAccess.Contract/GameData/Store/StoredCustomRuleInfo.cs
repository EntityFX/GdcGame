namespace EntityFX.Gdcame.DataAccess.Contract.MainServer.GameData.Store
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
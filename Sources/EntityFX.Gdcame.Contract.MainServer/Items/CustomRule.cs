namespace EntityFX.Gdcame.Contract.MainServer.Items
{
    using System.Runtime.Serialization;

    [DataContract]
    public class CustomRule
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }
    }
}
using System.Runtime.Serialization;

namespace EntityFX.Gdcame.Common.Contract.Items
{
    [DataContract]
    public class CustomRule
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Name { get; set; }

    }
}
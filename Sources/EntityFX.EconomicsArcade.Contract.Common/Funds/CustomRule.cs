using System.Runtime.Serialization;

namespace EntityFX.EconomicsArcade.Contract.Common.Funds
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
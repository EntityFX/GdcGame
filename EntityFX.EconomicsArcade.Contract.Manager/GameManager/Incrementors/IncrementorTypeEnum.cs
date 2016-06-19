using System.Runtime.Serialization;

namespace EntityFX.EconomicsArcade.Contract.Manager.GameManager.Incrementors
{
    [DataContract]
    public enum IncrementorTypeEnum
    {
        [EnumMember]
        ValueIncrementor,
        [EnumMember]
        PercentageIncrementor
    }
}

using System.Runtime.Serialization;

namespace EntityFX.EconomicsArcade.Contract.Common.Incrementors
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

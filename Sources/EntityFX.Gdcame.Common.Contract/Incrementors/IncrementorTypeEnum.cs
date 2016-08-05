using System.Runtime.Serialization;

namespace EntityFX.Gdcame.Common.Contract.Incrementors
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

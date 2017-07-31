namespace EntityFX.Gdcame.Contract.MainServer.Incrementors
{
    using System.Runtime.Serialization;

    [DataContract]
    public enum IncrementorTypeEnum
    {
        [EnumMember] ValueIncrementor,
        [EnumMember] PercentageIncrementor
    }
}
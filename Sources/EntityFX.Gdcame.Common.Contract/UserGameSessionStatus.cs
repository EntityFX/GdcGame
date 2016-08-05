using System.Runtime.Serialization;

namespace EntityFX.Gdcame.Common.Contract
{
    [DataContract]
    public enum UserGameSessionStatus
    {
        /// <summary>
        /// 
        /// </summary>
        [EnumMember]
        GameNotStarted,
        [EnumMember]
        Offline,
        [EnumMember]
        Online
    }
}
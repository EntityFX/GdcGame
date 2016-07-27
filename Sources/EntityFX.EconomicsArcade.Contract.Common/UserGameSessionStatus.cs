using System.Runtime.Serialization;

namespace EntityFX.EconomicsArcade.Contract.Common
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
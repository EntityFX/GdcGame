using System.Runtime.Serialization;

namespace EntityFX.EconomicsArcade.Contract.Manager
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
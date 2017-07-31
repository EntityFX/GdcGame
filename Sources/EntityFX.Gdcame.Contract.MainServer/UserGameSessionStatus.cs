namespace EntityFX.Gdcame.Contract.MainServer
{
    using System.Runtime.Serialization;

    [DataContract]
    public enum UserGameSessionStatus
    {
        /// <summary>
        /// </summary>
        [EnumMember] GameNotStarted,
        [EnumMember] Offline,
        [EnumMember] Online
    }
}
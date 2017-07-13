using System.Security.Principal;

namespace EntityFX.Gdcame.Manager.MainServer
{
    public class CustomGameIdentity : IIdentity
    {
        public string AuthenticationType { get; internal set; }
        public bool IsAuthenticated { get; internal set; }
        public string Name { get; internal set; }
    }
}
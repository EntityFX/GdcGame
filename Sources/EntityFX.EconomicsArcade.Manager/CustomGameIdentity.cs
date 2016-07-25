using System.Security.Principal;

namespace EntityFX.EconomicsArcade.Manager
{
    public class CustomGameIdentity : IIdentity
    {
        public string AuthenticationType { get; internal set; }
        public bool IsAuthenticated { get; internal set; }
        public string Name { get; internal set; }
    }
}
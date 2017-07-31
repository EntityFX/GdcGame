namespace EntityFX.Gdcame.Engine.Contract.GameEngine
{
    using System.Security.Principal;

    public class CustomGameIdentity : IIdentity
    {
        public string AuthenticationType { get; set; }
        public bool IsAuthenticated { get; set; }
        public string Name { get; set; }
    }
}
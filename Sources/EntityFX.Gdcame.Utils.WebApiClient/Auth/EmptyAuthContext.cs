using System;

namespace EntityFX.Gdcame.Utils.WebApiClient.Auth
{
    public class EmptyAuthContext : IAuthContext<object>
    {
        public object Context { get; set; }
        public Uri BaseUri { get; set; }
    }
}
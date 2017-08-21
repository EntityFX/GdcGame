namespace EntityFX.Gdcame.Infrastructure.Api.Exceptions
{
    using System;

    public class NoServerConnectionData : ErrorData
    {
        public Uri Uri { get; set; }
    }
}
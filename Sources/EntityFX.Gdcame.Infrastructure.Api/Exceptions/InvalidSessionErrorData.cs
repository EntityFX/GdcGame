namespace EntityFX.Gdcame.Infrastructure.Api.Exceptions
{
    using System;

    public class InvalidSessionErrorData : ErrorData
    {
        public Guid SessionGuid { get; set; }
    }
}
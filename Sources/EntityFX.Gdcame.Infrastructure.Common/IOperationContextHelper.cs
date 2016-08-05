using System;

namespace EntityFX.Gdcame.Infrastructure.Common
{
    public interface IOperationContext
    {
        Guid? SessionId { get; set; }
    }

    public interface IOperationContextHelper
    {
        IOperationContext Instance { get; }
    }
}

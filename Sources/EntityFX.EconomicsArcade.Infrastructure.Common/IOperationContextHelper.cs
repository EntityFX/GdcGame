using System;
namespace EntityFX.EconomicsArcade.Infrastructure.Common
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

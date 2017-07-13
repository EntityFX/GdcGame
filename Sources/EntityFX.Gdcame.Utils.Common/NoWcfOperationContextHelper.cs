using System;
using EntityFX.Gdcame.Infrastructure.Common;

namespace EntityFX.Gdcame.Utils.Common
{
    public class NoWcfOperationContextHelper : IOperationContext, IOperationContextHelper
    {
        private static readonly Lazy<IOperationContext> ObjInstance =
            new Lazy<IOperationContext>(() => new NoWcfOperationContextHelper());

        public Guid? SessionId { get; set; }

        public IOperationContext Instance
        {
            get { return ObjInstance.Value; }
        }
    }
}
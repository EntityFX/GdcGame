using System;
using EntityFX.Gdcame.Infrastructure.Common;

namespace EntityFX.Gdcame.Utils.ConsoleHostApp.AllInOneCore
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
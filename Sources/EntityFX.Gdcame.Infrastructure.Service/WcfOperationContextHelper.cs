using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using EntityFX.Gdcame.Infrastructure.Common;

namespace EntityFX.Gdcame.Infrastructure.Service
{
    public class WcfOperationContextHelper : IOperationContext, IOperationContextHelper
    {
        private static readonly Lazy<WcfOperationContextHelper> ObjInstance =
            new Lazy<WcfOperationContextHelper>(() => new WcfOperationContextHelper());

        private const string HeaderNs = "http://entityfx.ru";

        private const string SessionIdKey = "SessionId";

        public IOperationContext Instance
        {
            get { return ObjInstance.Value; }
        }


        public Guid? SessionId
        {
            get { return GetHeader<Guid?>(SessionIdKey); }
            set { SetHeader(SessionIdKey, value); }
        }

        private T GetHeader<T>(string header)
        {
            var messageHeaders = OperationContext.Current.IncomingMessageHeaders;
            var headerIndex = messageHeaders.FindHeader(header, HeaderNs);
            if (headerIndex < 0) return default(T);
            return messageHeaders.GetHeader<T>(headerIndex);
        }

        private void SetHeader<T>(string header, T value) 
        {
            var messageHeader = MessageHeader.CreateHeader(header, HeaderNs, value);
            OperationContext.Current.OutgoingMessageHeaders.Add(messageHeader);
        }
    }
}

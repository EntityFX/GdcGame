using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace EntityFX.EconomicsArcade.Infrastructure.Common
{
    public class OperationContextHelper
    {
        private static Lazy<OperationContextHelper> _instance =
            new Lazy<OperationContextHelper>(() => new OperationContextHelper());

        private const string HEADER_NS = "http://entityfx.ru";

        private const string SESSION_ID = "SessionId";

        public static OperationContextHelper Instance
        {
            get { return _instance.Value; }
        }


        public Guid? SessionId
        {
            get { return GetHeader<Guid?>(SESSION_ID); }
            set { SetHeader(SESSION_ID, value); }
        }

        private T GetHeader<T>(string header)
        {
            var messageHeaders = OperationContext.Current.IncomingMessageHeaders;
            var headerIndex = messageHeaders.FindHeader(header, HEADER_NS);
            if (headerIndex < 0) return default(T);
            return messageHeaders.GetHeader<T>(headerIndex);
        }

        private void SetHeader<T>(string header, T value) 
        {
            var messageHeader = MessageHeader.CreateHeader(header, HEADER_NS, value);
            OperationContext.Current.OutgoingMessageHeaders.Add(messageHeader);
        }
    }
}

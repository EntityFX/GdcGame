using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using EntityFX.Gdcame.Manager.Contract;
using EntityFX.Gdcame.Manager.Contract.SessionManager;

namespace EntityFX.Gdcame.Utils.Common
{
    public class InvalidSessionFaultHandler : IErrorHandler
    {
        /// <summary>
        /// Enables the creation of a custom <see cref="T:System.ServiceModel.FaultException`1"/> that is returned from an exception in the course of a service method.
        /// </summary>
        /// <param name="error">The <see cref="T:System.Exception"/> object thrown in the course of the service operation.</param><param name="version">The SOAP version of the message.</param><param name="fault">The <see cref="T:System.ServiceModel.Channels.Message"/> object that is returned to the client, or service, in the duplex case.</param>
        public void ProvideFault(Exception error, MessageVersion version, ref Message fault)
        {
            if (error is FaultException)
            {
                return;
            }
            var invalidSessionException = error as InvalidSessionException;
            if (invalidSessionException != null)
            {
                var ivalidSessionFaultException = new FaultException<InvalidSessionFault>(new InvalidSessionFault
                {
                    SessionGuid = invalidSessionException.SessionGuid
                }, new FaultReason(invalidSessionException.Message));
                var messageFault = ivalidSessionFaultException.CreateMessageFault();

                // Creates the new message based on the message fault
                fault = Message.CreateMessage(version, messageFault, ivalidSessionFaultException.Action);
            }
        }

        /// <summary>
        /// Enables error-related processing and returns a value that indicates whether the dispatcher aborts the session and the instance context in certain cases. 
        /// </summary>
        /// <returns>
        /// true if Windows Communication Foundation (WCF) should not abort the session (if there is one) and instance context if the instance context is not <see cref="F:System.ServiceModel.InstanceContextMode.Single"/>; otherwise, false. The default is false.
        /// </returns>
        /// <param name="error">The exception thrown during processing.</param>
        public bool HandleError(Exception error)
        {
            return true;
        }
    }
}
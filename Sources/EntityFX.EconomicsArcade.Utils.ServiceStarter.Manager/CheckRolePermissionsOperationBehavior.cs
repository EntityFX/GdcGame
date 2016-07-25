using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using EntityFX.EconomicsArcade.Contract.Manager;
using EntityFX.EconomicsArcade.Contract.Manager.AdminManager;
using EntityFX.EconomicsArcade.Contract.Manager.SessionManager;
using EntityFX.EconomicsArcade.Infrastructure.Common;
using EntityFX.EconomicsArcade.Manager;

namespace EntityFX.EconomicsArcade.Utils.ServiceStarter.Manager
{
    internal class CheckRolePermissionsOperationBehavior   : IOperationBehavior
    {
        private readonly GameSessions _gameSessions;

        public CheckRolePermissionsOperationBehavior(GameSessions gameSessions)
        {
            _gameSessions = gameSessions;
        }

        public void Validate(OperationDescription operationDescription)
        {

        }

        public void ApplyDispatchBehavior(OperationDescription operationDescription, DispatchOperation dispatchOperation)
        {
            dispatchOperation.Invoker = new CheckRolePermissionsInvoker(_gameSessions, dispatchOperation.Invoker, operationDescription.SyncMethod);
        }

        public void ApplyClientBehavior(OperationDescription operationDescription, ClientOperation clientOperation)
        {

        }

        public void AddBindingParameters(OperationDescription operationDescription, BindingParameterCollection bindingParameters)
        {
    
        }
    }

    internal class  CheckRolePermissionsInvoker : IOperationInvoker
    {
        private readonly IOperationInvoker _invoker;
        private readonly MethodInfo _methodInfo;
        private readonly GameSessions _gameSessions;
        public CheckRolePermissionsInvoker(GameSessions gameSessions, IOperationInvoker invoker, MethodInfo methodInfo)
        {
            _gameSessions = gameSessions;
            _invoker = invoker;
            _methodInfo = methodInfo;
        }

        public object[] AllocateInputs()
        {
            return _invoker.AllocateInputs();
        }

        public object Invoke(object instance, object[] inputs, out object[] outputs)
        {
            var attr = _methodInfo.GetCustomAttribute<CustomPrincipalPermissionAttribute>();
            if (attr != null)
            {
                var sessionId = OperationContextHelper.Instance.SessionId ?? default(Guid);
                var session = _gameSessions.GetSession(sessionId);
                if (!attr.AllowedRoles.Any(_ => session.UserRoles.Contains(_)))
                {
                    throw new FaultException<InsufficientPermissionsFault>(
                        new InsufficientPermissionsFault()
                        {
                            RequiredRoles = attr.AllowedRoles,
                            CurrentRoles = session.UserRoles
                        }
                        , string.Format("User {0} doesn't have enough permissions to perform this operation", session.Login));
                }
                            return _invoker.Invoke(instance, inputs, out outputs);
            }
            throw new FaultException<InsufficientPermissionsFault>(
                new InsufficientPermissionsFault() {RequiredRoles = null, CurrentRoles = new []{UserRole.GenericUser}}
                , string.Format("User doesn't have enough permissions to perform this operation"));
        }

        public IAsyncResult InvokeBegin(object instance, object[] inputs, AsyncCallback callback, object state)
        {
            return _invoker.InvokeBegin(instance, inputs, callback, state);
        }

        public object InvokeEnd(object instance, out object[] outputs, IAsyncResult result)
        {
            return _invoker.InvokeEnd(instance, out outputs, result);
        }

        public bool IsSynchronous
        {
            get { return _invoker.IsSynchronous; }
        }
    }
}
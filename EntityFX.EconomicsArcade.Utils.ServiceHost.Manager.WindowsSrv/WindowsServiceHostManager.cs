using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

using EntityFX.EconomicsArcade.Utils.ServiceStarter.Manager;

namespace EntityFX.EconomicsArcade.Utils.ServiceHost.Mananger.WindowsSrv
{
    public partial class WindowsServiceHostManager : ServiceBase
    {
        ManagerStarter _managerStarter;

        public WindowsServiceHostManager(ManagerStarter managerStarter)
        {
            InitializeComponent();

            _managerStarter = managerStarter;
        }

        protected override void OnStart(string[] args)
        {
            _managerStarter.StartService();

            AddLog("Service " + this.ServiceName + " is started");
        }

        protected override void OnStop()
        {
            _managerStarter.StopService();

            AddLog("Service " + this.ServiceName + " is stoped");
        }

        public void AddLog(string log)
        {
            try
            {
                if (!EventLog.SourceExists("MyExampleService"))
                {
                    EventLog.CreateEventSource("MyExampleService", "MyExampleService");
                }
                eventLogMain.Source = "MyExampleService";
                eventLogMain.WriteEntry(log);
            }
            catch { }
        }
    }
}

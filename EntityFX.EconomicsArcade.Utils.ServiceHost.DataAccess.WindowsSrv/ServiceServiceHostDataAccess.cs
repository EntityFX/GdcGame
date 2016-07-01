using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

using EntityFX.EconomicsArcade.Utils.ServiceStarter.DataAccess;

namespace EntityFX.EconomicsArcade.Utils.ServiceHost.DataAccess.WindowsSrv
{
    public partial class Service_ServiceHost_DataAccess : ServiceBase
    {
        DataAccessStarter _dataAccessStarter;

        public Service_ServiceHost_DataAccess(DataAccessStarter dataAccessStarter)
        {
            InitializeComponent();

            _dataAccessStarter = dataAccessStarter;
        }

        protected override void OnStart(string[] args)
        {
            _dataAccessStarter.StartService();

            AddLog("Service " + this.ServiceName + " is started");
        }

        protected override void OnStop()
        {
            _dataAccessStarter.StopService();

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

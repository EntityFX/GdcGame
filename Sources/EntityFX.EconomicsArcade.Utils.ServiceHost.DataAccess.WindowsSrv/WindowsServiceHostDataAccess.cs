using System.Diagnostics;
using System.ServiceProcess;
using EntityFX.Gdcame.Utils.ServiceStarter.DataAccess;

namespace EntityFX.EconomicsArcade.Utils.ServiceHost.DataAccess.WindowsSrv
{
    public partial class WindowsServiceHostDataAccess : ServiceBase
    {
        DataAccessStarter _dataAccessStarter;

        public WindowsServiceHostDataAccess(DataAccessStarter dataAccessStarter)
        {
            InitializeComponent();

            _dataAccessStarter = dataAccessStarter;
        }

        protected override void OnStart(string[] args)
        {
            _dataAccessStarter.StartServices();

            AddLog("Service " + this.ServiceName + " is started");
        }

        protected override void OnStop()
        {
            _dataAccessStarter.StopServices();

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

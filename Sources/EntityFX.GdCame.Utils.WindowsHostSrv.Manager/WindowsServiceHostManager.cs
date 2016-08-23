using System.Diagnostics;
using System.ServiceProcess;
using EntityFX.Gdcame.Utils.ServiceStarter.Manager;

namespace EntityFX.GdCame.Utils.WindowsHostSrv.Manager
{
    public partial class WindowsServiceHostManager : ServiceBase
    {
        private readonly ManagerStarter _managerStarter;

        public WindowsServiceHostManager(ManagerStarter managerStarter)
        {
            InitializeComponent();

            _managerStarter = managerStarter;
        }

        protected override void OnStart(string[] args)
        {
            _managerStarter.StartServices();

            AddLog("Service " + ServiceName + " is started");
        }

        protected override void OnStop()
        {
            _managerStarter.StopServices();

            AddLog("Service " + ServiceName + " is stoped");
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
            catch
            {
            }
        }
    }
}
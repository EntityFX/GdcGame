﻿using System.Diagnostics;
using System.ServiceProcess;
using EntityFX.Gdcame.Utils.ServiceStarter.WcfDataAccess;

namespace EntityFX.Gdcame.Utils.WindowsHostSrv.WcfDataAccess
{
    public partial class WindowsServiceHostDataAccess : ServiceBase
    {
        private readonly DataAccessStarter _dataAccessStarter;

        public WindowsServiceHostDataAccess(DataAccessStarter dataAccessStarter)
        {
            InitializeComponent();

            _dataAccessStarter = dataAccessStarter;
        }

        protected override void OnStart(string[] args)
        {
            _dataAccessStarter.StartServices();

            AddLog("Service " + ServiceName + " is started");
        }

        protected override void OnStop()
        {
            _dataAccessStarter.StopServices();

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
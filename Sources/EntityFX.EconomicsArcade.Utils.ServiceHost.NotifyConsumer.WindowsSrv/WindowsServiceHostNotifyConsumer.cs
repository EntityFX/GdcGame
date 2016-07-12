﻿using System.Diagnostics;
using System.ServiceProcess;
using EntityFX.EconomicsArcade.Utils.ServiceStarter.NotifyConsumer;

namespace EntityFX.EconomicsArcade.Utils.ServiceHost.NotifyConsumer.Temp
{
    public partial class WindowsServiceHostNotifyConsumer : ServiceBase
    {
        NotifyConsumerStarter _notifyConsumerStarter;

        public WindowsServiceHostNotifyConsumer(NotifyConsumerStarter notifyConsumerStarter)
        {
            InitializeComponent();

            _notifyConsumerStarter = notifyConsumerStarter;
        }

        protected override void OnStart(string[] args)
        {
            _notifyConsumerStarter.StartService();

            AddLog("Service " + this.ServiceName + " is started");
        }

        protected override void OnStop()
        {
            _notifyConsumerStarter.StopService();

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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

using EntityFX.EconomicsArcade.Utils.ServiceStarter.NotifyConsumer;
using System.Configuration;
using Microsoft.Owin.Hosting;

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

            string url = ConfigurationManager.AppSettings["NotifyConsumerSignalRHubEndpoint_AddressServiceUrl"];
            using (WebApp.Start(url))
            {
                AddLog("Server running on " + url);
                //Console.WriteLine("Server running on {0}", url);
                //Console.ReadLine();
            }

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

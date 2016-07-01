using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.Threading.Tasks;

using System.Messaging;
using System.Configuration;

namespace EntityFX.EconomicsArcade.Utils.ServiceHost.DataAccess.WindowsSrv
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
        }

        private void checkAndCreateMsmqQueue()
        {
            try
            {
                string msmqHandlingPath = ConfigurationManager.AppSettings["DataAccessHost_MsmqHandlingPath"];
                string msmqLabel = ConfigurationManager.AppSettings["DataAccessHost_MsmqLabel"];
                bool msmqUseJournalQueue = Convert.ToBoolean( ConfigurationManager.AppSettings["DataAccessHost_MsmqUseJournalQueue"] );
                int msmqMaximumJournalSize = Convert.ToInt32( ConfigurationManager.AppSettings["DataAccessHost_MsmqMaximumJournalSize"] );
                int msmqMaximumQueueSize = Convert.ToInt32( ConfigurationManager.AppSettings["DataAccessHost_MsmqMaximumQueueSize"] );
                bool isTransational = true;

                if (!MessageQueue.Exists(msmqHandlingPath))
                    MessageQueue.Create(msmqHandlingPath, isTransational);
            }
            catch (Exception exp)
            {
                Console.WriteLine("Error: " + exp.Message);
            }
        }

        private void serviceInstallerMain_AfterInstall(object sender, InstallEventArgs e)
        {
            checkAndCreateMsmqQueue();
        }

        private void serviceProcessInstallerMain_AfterInstall(object sender, InstallEventArgs e)
        {

        }
    }
}

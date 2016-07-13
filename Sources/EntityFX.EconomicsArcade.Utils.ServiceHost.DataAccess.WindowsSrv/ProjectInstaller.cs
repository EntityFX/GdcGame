using System;
using System.ComponentModel;
using System.Configuration.Install;
using System.Messaging;
using System.Configuration;
using System.Reflection;

namespace EntityFX.EconomicsArcade.Utils.ServiceHost.DataAccess.WindowsSrv
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
        }

        private void CheckAndCreateMsmqQueue()
        {

            var config = ConfigurationManager.OpenExeConfiguration(Assembly.GetAssembly(typeof(ProjectInstaller)).Location);

            try
            {
                string msmqHandlingPath = config.AppSettings.Settings["DataAccessHost_MsmqHandlingPath"].Value;
                string msmqLabel = config.AppSettings.Settings["DataAccessHost_MsmqLabel"].Value;
                bool msmqUseJournalQueue = Convert.ToBoolean(config.AppSettings.Settings["DataAccessHost_MsmqUseJournalQueue"].Value);
                int msmqMaximumJournalSize = Convert.ToInt32(config.AppSettings.Settings["DataAccessHost_MsmqMaximumJournalSize"].Value);
                int msmqMaximumQueueSize = Convert.ToInt32(config.AppSettings.Settings["DataAccessHost_MsmqMaximumQueueSize"].Value);
                bool isTransational = true;

                Console.WriteLine("msmqHandlingPath is {0}", msmqHandlingPath);
                Console.WriteLine("msmqLabel is {0}", msmqLabel);
                Console.WriteLine("msmqUseJournalQueue is {0}", msmqUseJournalQueue);
                Console.WriteLine("msmqMaximumJournalSize is {0}", msmqMaximumJournalSize);
                Console.WriteLine("msmqMaximumQueueSize is {0}", msmqMaximumQueueSize);
                Console.WriteLine("isTransational is {0}", isTransational);

                if (!MessageQueue.Exists(msmqHandlingPath))
                   MessageQueue.Create(msmqHandlingPath, isTransational);
            }
            catch (Exception exp)
            {
                Console.WriteLine("!!!Error TREE!!!");
                Console.WriteLine("Error: " + exp.Message);
            }
        }

        private void serviceInstallerMain_AfterInstall(object sender, InstallEventArgs e)
        {
            Console.WriteLine("!!!Creating MSMQ!!!");
            CheckAndCreateMsmqQueue();
            Console.WriteLine("!!!Finishing creating MSMQ!!!");
        }

        private void serviceProcessInstallerMain_AfterInstall(object sender, InstallEventArgs e)
        {

        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.Threading.Tasks;

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

        private void checkAndCreateMsmqQueue()
        {

            var config = ConfigurationManager.OpenExeConfiguration(Assembly.GetAssembly(typeof(ProjectInstaller)).Location);

            Console.WriteLine(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
            Console.WriteLine("AppConfig count", ConfigurationManager.AppSettings.Keys.Count);
            Console.WriteLine(ConfigurationManager.AppSettings.Keys.Count);

            try
            {
                string msmqHandlingPath = config.AppSettings.Settings["DataAccessHost_MsmqHandlingPath"].Value;
                string msmqLabel = config.AppSettings.Settings["DataAccessHost_MsmqLabel"].Value;
                bool msmqUseJournalQueue = Convert.ToBoolean(config.AppSettings.Settings["DataAccessHost_MsmqUseJournalQueue"].Value);
                int msmqMaximumJournalSize = Convert.ToInt32(config.AppSettings.Settings["DataAccessHost_MsmqMaximumJournalSize"].Value);
                int msmqMaximumQueueSize = Convert.ToInt32(config.AppSettings.Settings["DataAccessHost_MsmqMaximumQueueSize"].Value);
                bool isTransational = true;
                //string msmqHandlingPath = ConfigurationManager.AppSettings["DataAccessHost_MsmqHandlingPath"];
                //string msmqLabel = ConfigurationManager.AppSettings["DataAccessHost_MsmqLabel"];
                //bool msmqUseJournalQueue = Convert.ToBoolean( ConfigurationManager.AppSettings["DataAccessHost_MsmqUseJournalQueue"] );
                //int msmqMaximumJournalSize = Convert.ToInt32( ConfigurationManager.AppSettings["DataAccessHost_MsmqMaximumJournalSize"] );
                //int msmqMaximumQueueSize = Convert.ToInt32( ConfigurationManager.AppSettings["DataAccessHost_MsmqMaximumQueueSize"] );
                //bool isTransational = true;

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
            checkAndCreateMsmqQueue();
            Console.WriteLine("!!!Finishing creating MSMQ!!!");
        }

        private void serviceProcessInstallerMain_AfterInstall(object sender, InstallEventArgs e)
        {

        }
    }
}

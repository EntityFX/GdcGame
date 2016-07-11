namespace EntityFX.EconomicsArcade.Utils.ServiceHost.DataAccess.WindowsSrv
{
    partial class ProjectInstaller
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.serviceProcessInstallerMain = new System.ServiceProcess.ServiceProcessInstaller();
            this.serviceInstallerMain = new System.ServiceProcess.ServiceInstaller();
            // 
            // serviceProcessInstallerMain
            // 
            this.serviceProcessInstallerMain.Password = null;
            this.serviceProcessInstallerMain.Username = null;
            this.serviceProcessInstallerMain.AfterInstall += new System.Configuration.Install.InstallEventHandler(this.serviceProcessInstallerMain_AfterInstall);
            // 
            // serviceInstallerMain
            // 
            this.serviceInstallerMain.ServiceName = "EntityFX.EconomicsArcade.Utils.ServiceHost.DataAccess.WindowsSrv";
            this.serviceInstallerMain.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            this.serviceInstallerMain.AfterInstall += new System.Configuration.Install.InstallEventHandler(this.serviceInstallerMain_AfterInstall);
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.serviceProcessInstallerMain,
            this.serviceInstallerMain});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller serviceProcessInstallerMain;
        private System.ServiceProcess.ServiceInstaller serviceInstallerMain;
    }
}
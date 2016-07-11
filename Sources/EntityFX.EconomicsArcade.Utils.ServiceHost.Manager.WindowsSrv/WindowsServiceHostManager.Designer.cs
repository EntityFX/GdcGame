namespace EntityFX.EconomicsArcade.Utils.ServiceHost.Mananger.WindowsSrv
{
    partial class WindowsServiceHostManager
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
            this.eventLogMain = new System.Diagnostics.EventLog();
            ((System.ComponentModel.ISupportInitialize)(this.eventLogMain)).BeginInit();
            // 
            // Service_ServiceHost_Manager
            // 
            this.ServiceName = "EntityFX.EconomicsArcade.Utils.ServiceHost.Manager.WindowsSrv";
            ((System.ComponentModel.ISupportInitialize)(this.eventLogMain)).EndInit();

        }

        #endregion

        private System.Diagnostics.EventLog eventLogMain;
    }
}

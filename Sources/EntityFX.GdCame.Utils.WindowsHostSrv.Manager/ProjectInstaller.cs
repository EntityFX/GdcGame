using System;
using System.ComponentModel;

namespace EntityFX.GdCame.Utils.WindowsHostSrv.Manager
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();

            Console.WriteLine("Initialization");
        }
    }
}

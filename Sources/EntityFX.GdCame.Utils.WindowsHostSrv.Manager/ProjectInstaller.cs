using System;
using System.ComponentModel;
using System.Configuration.Install;

namespace EntityFX.GdCame.Utils.WindowsHostSrv.Manager
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();

            Console.WriteLine("Initialization");
        }
    }
}
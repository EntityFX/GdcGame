using System;
using System.ComponentModel;
using System.Configuration.Install;

namespace EntityFX.Gdcame.Utils.WindowsHostSrv.WcfManager
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
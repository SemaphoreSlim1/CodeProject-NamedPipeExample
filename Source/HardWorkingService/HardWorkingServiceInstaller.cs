using HardWorkingService.Install;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace HardWorkingService
{
    [RunInstaller(true)]
    public partial class HardWorkingServiceInstaller : System.Configuration.Install.Installer
    {
        public HardWorkingServiceInstaller() : base()
        {            
            var serviceProcessInstaller = new ServiceProcessInstaller();
            var serviceInstaller = new ServiceInstaller();

            //Service Account Information
            serviceProcessInstaller.Account = ServiceAccount.LocalSystem;
            serviceProcessInstaller.Username = null;
            serviceProcessInstaller.Password = null;

            //Service Information
            serviceInstaller.DisplayName = InstallTimeConfigurationManager.GetConfigurationValue("ServiceDisplayName");
            serviceInstaller.Description = InstallTimeConfigurationManager.GetConfigurationValue("ServiceDescription");
            serviceInstaller.StartType = ServiceStartMode.Automatic;
            serviceInstaller.DelayedAutoStart = true;


            //This must be identical to the WindowsService.ServiceBase name
            //set in the constructor of WindowsService.cs
            serviceInstaller.ServiceName = InstallTimeConfigurationManager.GetConfigurationValue("SystemServiceName");

            this.Installers.Add(serviceProcessInstaller);
            this.Installers.Add(serviceInstaller);

            this.Committed += Installer_Committed;
        }

        private void Installer_Committed(Object sender, InstallEventArgs e)
        {
            //auto start the service once the installation is finished
            var controller = new ServiceController(InstallTimeConfigurationManager.GetConfigurationValue("SystemServiceName"));
            controller.Start();
        }
    }
}

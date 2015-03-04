using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HardWorkingService.Install
{
    internal static class InstallTimeConfigurationManager
    {
        public static string GetConfigurationValue(string key)
        {
            Assembly service = Assembly.GetAssembly(typeof(HardWorkingServiceInstaller));
            var config = ConfigurationManager.OpenExeConfiguration(service.Location);

            return config.AppSettings.Settings[key].Value;
        }
    }
}

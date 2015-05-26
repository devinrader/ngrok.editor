using GalaSoft.MvvmLight;
using ngrok.editor.desktop.wpf.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ngrok.editor.desktop.wpf
{
    public class ConfigurationService : ViewModelBase, IConfigurationService
    {
        const string CONFIGFILENAME = "applicationhost.config";

        ApplicationConfiguration _applicationConfiguration;
        Configuration _configuration;
        bool _forceConfiguration = false;

        public ConfigurationService()
        {
            _configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            _applicationConfiguration = new ApplicationConfiguration();
            _applicationConfiguration.ApplicationHostsConfigPath = _configuration.AppSettings.Settings.GetValueOrDefault("applicationHostsConfigPath");
            _applicationConfiguration.NgrokExecutablePath = _configuration.AppSettings.Settings.GetValueOrDefault("ngrokExecutablePath");

            //set a default applicationhosts.config location if there is no value and we find the location exists
            if (string.IsNullOrWhiteSpace(_applicationConfiguration.ApplicationHostsConfigPath)) {

                string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"IISExpress\config\");

                if (File.Exists(Path.Combine(path, "applicationhost.config")))
                {
                    _applicationConfiguration.ApplicationHostsConfigPath = path;
                }
            }

            this.RaisePropertyChanged("IsConfigurationNeeded");
        }

        public ApplicationConfiguration ApplicationConfiguration
        {
            get
            {
                return _applicationConfiguration;
            }
        }

        public string ApplicationHostConfigPath {
            get
            {
                return Path.Combine(this.ApplicationConfiguration.ApplicationHostsConfigPath, CONFIGFILENAME);
            }
        }

        public void Save()
        {
            _configuration.AppSettings.Settings.CreateOrUpdateKey("applicationHostsConfigPath", this.ApplicationConfiguration.ApplicationHostsConfigPath);
            _configuration.AppSettings.Settings.CreateOrUpdateKey("ngrokExecutablePath", this.ApplicationConfiguration.NgrokExecutablePath);
            _configuration.Save();

            this._forceConfiguration = false;
            this.RaisePropertyChanged("IsConfigurationNeeded");

        }

        public void ForceConfiguration()
        { 
            this._forceConfiguration = true;
            this.RaisePropertyChanged("IsConfigurationNeeded");
        }

        public bool IsForced { get { return _forceConfiguration; } }

        public bool IsConfigurationNeeded
        {
            get
            {

                if (!this.IsConfigurationValid || this._forceConfiguration)
                {
                    return true;
                }

                return false;
            }
        }

        public bool IsConfigurationValid
        {
            get
            {
                if (
                    string.IsNullOrWhiteSpace(this.ApplicationConfiguration.ApplicationHostsConfigPath) ||
                    !File.Exists(Path.Combine(this.ApplicationConfiguration.ApplicationHostsConfigPath, "applicationhost.config")) ||
                    string.IsNullOrWhiteSpace(this.ApplicationConfiguration.NgrokExecutablePath) ||
                    !File.Exists(Path.Combine(this.ApplicationConfiguration.NgrokExecutablePath, "ngrok.exe"))
                    )
                {
                    return false;
                }

                return true;
            }
        }

    }
}

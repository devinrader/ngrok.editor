using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ngrok.editor.desktop.wpf;

namespace ngrok.editor.desktop.wpf.Model
{
    public class ApplicationConfiguration : ViewModelBase
    {
        private string _applicationHostsConfigPath, _ngrokConfigPath, _ngrokExecutablePath;

        public string ApplicationHostsConfigPath {
            get { return _applicationHostsConfigPath; }
            set 
            { 
                _applicationHostsConfigPath = value;
                this.RaisePropertyChanged("ApplicationHostsConfigPath");
                
            }
        }

        public string NgrokConfigPath {
            get { return _ngrokConfigPath; }
            set
            {
                _ngrokConfigPath = value;
                this.RaisePropertyChanged("NgrokConfigPath");
            }
        }

        public string NgrokExecutablePath {
            get { return _ngrokExecutablePath; }
            set
            {
                _ngrokExecutablePath = value;
                this.RaisePropertyChanged("NgrokExecutablePath");
            }
        }
    }
}

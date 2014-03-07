using GalaSoft.MvvmLight;
using ngrok.editor.desktop.wpf.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ngrok.editor.desktop.wpf.Model
{
    public class UrlAcl : ViewModelBase
    {
        string _urlprefix = String.Empty;

        public UrlAcl(HTTP_SERVICE_CONFIG_URLACL_SET urlacl)
        {
            this.UrlPrefix = urlacl.KeyDesc.pUrlPrefix;

            var match = Regex.Match(this.UrlPrefix, ":([0-9]+?)/");
            var port = match.Groups[1].Value;

            this.Port = port;
        }

        public string UrlPrefix {
            get { return _urlprefix; }
            private set {
                _urlprefix = value;
                this.RaisePropertyChanged("UrlPrefix");
            } 
        }

        public string Port
        {
            get;
            private set;
        }
    }
}

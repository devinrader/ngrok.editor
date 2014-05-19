using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using ngrok.editor.desktop.wpf.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ngrok.editor.desktop.wpf.ViewModel
{
    public class SitesViewModel : ViewModelBase
    {
        IConfigurationService _configurationService;
        ISiteDataService _siteDataService;
        SiteViewModel _selectedSite;

        public SitesViewModel(ISiteDataService siteDataService, IConfigurationService configurationService)
        {
            _siteDataService = siteDataService;
            _configurationService = configurationService;

            _siteDataService.PropertyChanged += (s,e) => {
                if (e.PropertyName == "Sites")
                {
                    this.SelectedSite = this.Sites.FirstOrDefault();
                }
            };

            this.LoadNgrokInspector = new RelayCommand(() => {
                Process.Start("http://localhost:4040/");
            });

            this.RunNgrokCommand = new RelayCommand(() => {

                Process p = new Process();
                p.StartInfo = new ProcessStartInfo( "cmd.exe" ) 
                    {
                        Arguments = string.Format("/k \"{0}\" -subdomain={1} {2}", Path.Combine(_configurationService.ApplicationConfiguration.NgrokExecutablePath, "ngrok.exe"), this.SelectedSite.Subdomain, this.SelectedSite.LocalhostPort),
                        UseShellExecute = true,
                        WindowStyle = ProcessWindowStyle.Normal
                    };
                p.Start();

            });
        }

        public RelayCommand RunNgrokCommand { get; set; }
        public RelayCommand LoadNgrokInspector { get; set; }

        public ISiteDataService SitesDataService
        {
            get { return _siteDataService; }
        }

        public ObservableCollection<SiteViewModel> Sites
        {
            get { return _siteDataService.Sites; }
        }

        public SiteViewModel SelectedSite
        {
            get { return _selectedSite; }
            set
            {
                if (value != this._selectedSite)
                {
                    this._selectedSite = value;
                    this.RaisePropertyChanged("SelectedSite");
                }
            }
        }        
    }
}

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
        string _siteFilter;
        ObservableCollection<SiteViewModel> _filteredSites = new ObservableCollection<SiteViewModel>();

        public SitesViewModel(ISiteDataService siteDataService, IConfigurationService configurationService)
        {
            _siteDataService = siteDataService;
            _configurationService = configurationService;

            _siteDataService.PropertyChanged += (s,e) => {
                if (e.PropertyName == "Sites")
                {
                    this.SiteFilter = String.Empty;
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
                        Arguments = string.Format("/k \"{0}\" http -subdomain={1} {2}", Path.Combine(_configurationService.ApplicationConfiguration.NgrokExecutablePath, "ngrok.exe"), this.SelectedSite.Subdomain, this.SelectedSite.LocalhostPort),
                        UseShellExecute = true,
                        WindowStyle = ProcessWindowStyle.Normal
                    };
                p.Start();

            });

            this.SiteFilter = String.Empty;
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

        public ObservableCollection<SiteViewModel> FilteredSites
        {
            get {
                return _filteredSites;
            }
        }

        public string SiteFilter
        {
            get { 
                return _siteFilter; 
            }
            set {
                if (value != this._siteFilter)
                {
                    this._siteFilter = value;

                    if (!string.IsNullOrWhiteSpace(this._siteFilter))
                    {
                        _filteredSites = new ObservableCollection<SiteViewModel>(this.Sites.Where(s => s.name.Contains(this.SiteFilter)));
                    }
                    else
                    {
                        _filteredSites = this.Sites;
                    }
                    this.RaisePropertyChanged("SiteFilter");
                    this.RaisePropertyChanged("FilteredSites");
                }
            }
        }
    }
}

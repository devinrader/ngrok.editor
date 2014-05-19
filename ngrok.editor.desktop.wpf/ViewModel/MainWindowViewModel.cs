using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using ngrok.editor.desktop.wpf.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ngrok.editor.desktop.wpf.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        IConfigurationService _configurationService;

        public MainWindowViewModel(IConfigurationService configurationService)
        {
            _configurationService = configurationService;

            this.QuitCommand = new RelayCommand(() =>
            {
                App.Current.Shutdown(0);
            });

            this.ConfigureCommand = new RelayCommand(() =>
            {
                this.ConfigurationService.ForceConfiguration();
            });
        }

        public IConfigurationService ConfigurationService { get { return _configurationService; } }

        public RelayCommand QuitCommand { get; set; }

        public RelayCommand ConfigureCommand { get; set; }
    }
}

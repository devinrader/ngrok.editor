using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using ngrok.editor.desktop.wpf.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ngrok.editor.desktop.wpf.ViewModel
{
    public class ApplicationConfigurationViewModel : ViewModelBase
    {
        IConfigurationService _configurationService;

        public ApplicationConfigurationViewModel(IConfigurationService configurationService)
        {
            _configurationService = configurationService;

            this.ApplicationHostsPathCommand = new RelayCommand(() =>
            {
                MessengerInstance.Send(new FolderDialogMessage((path) =>
                {
                    this.ApplicationConfiguration.ApplicationHostsConfigPath = path;
                }));
            });
            this.NgrokExecutablePathCommand = new RelayCommand(() =>
            {
                MessengerInstance.Send(new FolderDialogMessage((path) =>
                {
                    this.ApplicationConfiguration.NgrokExecutablePath = path;
                }));
            });
            this.SaveConfigurationCommand = new RelayCommand(
                () =>
                {
                    this._configurationService.Save();
                },
                () =>
                {
                    return this._configurationService.IsConfigurationValid;
                });
        }

        public RelayCommand ApplicationHostsPathCommand { get; set; }
        public RelayCommand NgrokExecutablePathCommand { get; set; }
        public RelayCommand SaveConfigurationCommand { get; set; }

        public ApplicationConfiguration ApplicationConfiguration
        {
            get { return _configurationService.ApplicationConfiguration; }
        }
    }
}

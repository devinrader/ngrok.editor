using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ngrok.editor.desktop.wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            //base.OnStartup(e);

            //var _configurationService = SimpleIoc.Default.GetInstance<IConfigurationService>();

            //if (_configurationService.IsConfigurationNeeded)
            //{
            //    //var window = new ConfigurationWindow();
            //    //window.Show();
            //}
            //else
            //{
                //var window = new MainWindow();
                //window.Show();
            //}
        }
    }
}

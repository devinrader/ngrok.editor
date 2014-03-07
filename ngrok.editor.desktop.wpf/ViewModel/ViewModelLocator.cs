/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:ngrok.editor.desktop.wpf"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;

namespace ngrok.editor.desktop.wpf.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            ////if (ViewModelBase.IsInDesignModeStatic)
            ////{
            ////    // Create design time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DesignDataService>();
            ////}
            ////else
            ////{
            ////    // Create run time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DataService>();
            ////}

            //we only ever need one instance of this

            if (!SimpleIoc.Default.IsRegistered<IConfigurationService>())
                SimpleIoc.Default.Register<IConfigurationService>(()=> new ConfigurationService());

            SimpleIoc.Default.Register<ISiteDataService, SiteDataService>();
            SimpleIoc.Default.Register<IUrlAclDataService, UrlAclDataService>();

            SimpleIoc.Default.Register<MainWindowViewModel>();
            SimpleIoc.Default.Register<ApplicationConfigurationViewModel>();
            SimpleIoc.Default.Register<SitesViewModel>();
        }

        public MainWindowViewModel MainWindow
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainWindowViewModel>();
            }
        }

        public ApplicationConfigurationViewModel ApplicationConfigurationUserControl
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ApplicationConfigurationViewModel>();
            }
        }

        public SitesViewModel SitesUserControl
        {
            get
            {
                return ServiceLocator.Current.GetInstance<SitesViewModel>();
            }
        }
        
        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}
using GalaSoft.MvvmLight;
using ngrok.editor.desktop.wpf.Model;
using ngrok.editor.desktop.wpf.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace ngrok.editor.desktop.wpf
{
    public class SiteDataService : ViewModelBase, ISiteDataService
    {
        IConfigurationService _configurationService;
        ObservableCollection<SiteViewModel> _sites;

        public SiteDataService(IConfigurationService configurationService)
        {
            _configurationService = configurationService;

            this.Sites = new ObservableCollection<SiteViewModel>();

            _configurationService.PropertyChanged += ApplicationConfiguration_PropertyChanged;

            if (_configurationService.IsConfigurationValid)
            {
                Load();
            }
        }

        void ApplicationConfiguration_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsConfigurationNeeded")
            {
                Load();
            }
        }

        public ObservableCollection<SiteViewModel> Sites
        {
            get
            {
                if (_sites == null)
                {
                    Load();
                }

                return _sites; 
            }
            private set {
                if (_sites != value)
                {
                    _sites = value;
                    this.RaisePropertyChanged("Sites");
                }
            }
        }

        public void Save() { }

       

        /// <summary>
        /// Deserializes the <sites /> section from the applicationhosts.config file into an ObservableCollection&lt;site&gt;
        /// </summary>
        private void Load()
        {
            if (File.Exists(_configurationService.ApplicationHostConfigPath))
            {
                var doc = XDocument.Load(_configurationService.ApplicationHostConfigPath);

                var events = new XmlDeserializationEvents();
                events.OnUnknownAttribute = (s, e) => { Debug.WriteLine("Unknown Attributed"); };
                events.OnUnknownElement = (s, e) => { Debug.WriteLine("Unknwon Element: " + e.Element.Name); };
                events.OnUnknownNode = (s, e) => { Debug.WriteLine("Unknown Node: " + e.Name); };
                events.OnUnreferencedObject = (s, e) => { Debug.WriteLine("Unreferenced Object"); };

                var deserializer = new XmlSerializer(typeof(List<site>), new XmlRootAttribute("sites"));

                var sitesElement = doc.Descendants("sites").FirstOrDefault();

                if (sitesElement != null)
                {
                    var reader = sitesElement.CreateReader();
                    var s = (List<site>)deserializer.Deserialize(reader, events);

                    //s = s.Select(site =>
                    //{
                    //    var node = doc.Descendants().Where(d => (d.Name == "site") && (d.Attribute("id").Value == site.id)).FirstOrDefault();
                    //    if (node != null)
                    //    {

                    //        var comment = node.Elements().DescendantNodesAndSelf().Where(n => n.NodeType == XmlNodeType.Comment).Select(c => c as XComment).FirstOrDefault();

                    //        if (comment != null)
                    //        {
                    //            site.subdomain = comment.Value;
                    //        }
                    //        else
                    //        {
                    //            site.subdomain = site.name;
                    //        }
                    //    }
                    //    return site;
                    //}).ToList();

                    var svms = s.Select(site => new SiteViewModel(site)).ToList();
                    this.Sites = new ObservableCollection<SiteViewModel>(svms);
                }
            }
        }
    }
}

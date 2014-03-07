using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using ngrok.editor.desktop.wpf.Model;
using ngrok.editor.desktop.wpf.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ngrok.editor.desktop.wpf.ViewModel
{
    public class SiteViewModel : ViewModelBase, IDataErrorInfo
    {
        IConfigurationService _configurationService;
        IUrlAclDataService _urlAclDataService;

        site _site;
        bool _hasApplicationBinding, _hasUrlAcl;
        string _subdomain;
        ObservableCollection<BindingViewModel> _bindings;

        public SiteViewModel(site site)
        {
            _site = site;

            var bvm = _site.bindings.Select(b => new BindingViewModel(b));
            _bindings = new ObservableCollection<BindingViewModel>(bvm);

            _configurationService = SimpleIoc.Default.GetInstance<IConfigurationService>();

            var binding = this.bindings.Where(b => b.Host.Contains("ngrok.com")).FirstOrDefault();

            var replaced = Regex.Replace(_site.name, "[^A-Za-z0-9_]", "");
            this.Subdomain = (binding == null || string.IsNullOrWhiteSpace(binding.Subdomain)) ? replaced : binding.Subdomain;

            _hasApplicationBinding = binding != null;

            _urlAclDataService = SimpleIoc.Default.GetInstance<IUrlAclDataService>();
            _hasUrlAcl = _urlAclDataService.UrlAcls.Any(u=>u.Port==this.LocalhostPort && u.UrlPrefix.Contains("ngrok.com"));
        }

        public SiteViewModel(site site, bool hasUrlAcl) : this(site)
        {
            _hasUrlAcl = hasUrlAcl;
        }

        public string name { get { return _site.name; } }

        public string id { get { return _site.id; } }

        public bool serverAutoStart { get { return _site.serverAutoStart; } }

        public application application { get { return _site.application; } }

        public ObservableCollection<BindingViewModel> bindings { get { return _bindings; } }

        public bool HasApplicationBinding
        {
            get
            {
                return _hasApplicationBinding;
            }
            set
            {
                this.RaisePropertyChanged("Subdomain");

                if (IsSubdomainValid)
                {
                    if (value == false)
                    {
                        RemoveApplicationHostBinding();
                        this.RaisePropertyChanged("HasApplicationBinding");
                    }
                    else
                    {
                        try
                        {
                            AddApplicationHostBinding();
                            this.RaisePropertyChanged("HasApplicationBinding");
                        }
                        catch (Exception exc)
                        {
                            MessengerInstance.Send(new DialogMessage(exc.Message, c => { }));
                        }
                    }
                }
            }
        }

        public bool HasUrlAcl
        {
            get
            {
                return _hasUrlAcl;
            }
            set
            {
                this.RaisePropertyChanged("Subdomain");

                if (IsSubdomainValid)
                {
                    if (value == false)
                    {
                        RemoveUrlAcl();
                        this.RaisePropertyChanged("HasUrlAcl");
                    }
                    else
                    {
                        SetUrlAcl();
                        this.RaisePropertyChanged("HasUrlAcl");
                    }
                }
            }
        }

        public string LocalhostPort
        {
            get
            {
                var binding = this.bindings.FirstOrDefault(b => b.Host.Contains("localhost"));
                if (binding == null)
                    throw new Exception("No binding for 'localhost' could be found for site '{0}'");

                return binding.Port;
            }
        }

        public string Subdomain { 
            get { return _subdomain; }
            set
            {
                if (value != _subdomain)
                {
                    _subdomain = value;
                    RaisePropertyChanged("Subdomain");
                }
            }
        }

        /// <summary>
        /// Lazy way of adding a new <binding /> element to a site.  Load the config file, add the element, then add the cooresponding object to the site bindings list
        /// </summary>
        /// <param name="subdomain"></param>
        private void AddApplicationHostBinding()
        {
            string bindingInfo = string.Format("*:{0}:{1}.ngrok.com", this.LocalhostPort, this.Subdomain);

            var doc = XDocument.Load(_configurationService.ApplicationHostConfigPath); //load the applicationhost.config file

            var node = doc.Descendants().Where(d => (d.Name == "site") && (d.Attribute("id").Value == _site.id)).FirstOrDefault(); //find the correct site

            //make sure the site exists
            if (node == null)
                throw new Exception("The specified site '{0}' could not be found in the configuration file");

            //make sure an ngrok binding for this site does not already exist
            if (node.Descendants().Where(d => (d.Attribute("bindingInformation")!=null && d.Attribute("bindingInformation").Value == bindingInfo)).FirstOrDefault() != null)
                throw new Exception(string.Format("A binding with ngrok bindingInfo '{0}' is already present for site '{1}'", bindingInfo, _site.name));

            //create and add the new ngrok binding to the config file
            //XNamespace ng = "http://devinrader.info/applicationhost/extensions/ngrok/2014/1/";
            //var ns = new XAttribute(XNamespace.Xmlns + "ng", "http://devinrader.info/applicationhost/extensions/ngrok/2014/1/");
            //var sd = new XAttribute(ng + "subdomain", this.Subdomain);

            var binding = new XElement("binding", new XAttribute("protocol", "http"), new XAttribute("bindingInformation", bindingInfo));

            //node.Add(ns, sd);
            //node.Add(new XComment("subdomain:" + this.Subdomain));

            node.Element("bindings").Add(binding);
            doc.Save(_configurationService.ApplicationHostConfigPath);

            //create and add the new ngrok binding to this sites bindings collection
            _site.bindings.Add(new binding() { bindingInformation = bindingInfo, protocol = "http" });

            _hasApplicationBinding = true;
        }

        private void RemoveApplicationHostBinding()
        {
            var doc = XDocument.Load(_configurationService.ApplicationHostConfigPath);

            //make sure the site exists
            var node = doc.Descendants().Where(d => (d.Name == "site") && (d.Attribute("id").Value == _site.id)).FirstOrDefault();

            if (node == null)
                throw new Exception("The specified site '{0}' could not be found in the configuration file");
            
            var sourcebinding = node.Descendants().Where(n => n.Name == "binding" && n.Attribute("bindingInformation").Value.Contains("ngrok.com"));
            if (sourcebinding.Count() <= 0)
                throw new Exception("An XML binding with ngrok info '{0}' could not be found for site '{1}'");

            var localbinding = _site.bindings.FirstOrDefault(b => b.bindingInformation.Contains("ngrok.com"));
            if (localbinding==null)
                throw new Exception("A local binding with ngrok info '{0}' could not be found for site '{1}'");

            sourcebinding.Remove();
            doc.Save(_configurationService.ApplicationHostConfigPath);
            _site.bindings.Remove(localbinding);

            _hasApplicationBinding = false;
        }

        private void SetUrlAcl()
        {
            var retVal = PInvoke.HttpInitialize(new HTTPAPI_VERSION(2, 0), PInvoke.HTTP_INITIALIZE_CONFIG, IntPtr.Zero);

            if ((uint)PInvoke.NO_ERROR != retVal)
            {
                throw new Exception(string.Format("Could not set the initialize the HTTP API.  Code {0}", retVal));
            }

            var url = string.Format("http://{0}.ngrok.com:{1}/", this.Subdomain, this.LocalhostPort);

            HTTP_SERVICE_CONFIG_URLACL_KEY keyDesc = new HTTP_SERVICE_CONFIG_URLACL_KEY(url);
            HTTP_SERVICE_CONFIG_URLACL_PARAM paramDesc = new HTTP_SERVICE_CONFIG_URLACL_PARAM("D:(A;;GX;;;WD)");

            HTTP_SERVICE_CONFIG_URLACL_SET inputConfigInfoSet = new HTTP_SERVICE_CONFIG_URLACL_SET();
            inputConfigInfoSet.KeyDesc = keyDesc;
            inputConfigInfoSet.ParamDesc = paramDesc;

            IntPtr pInputConfigInfo = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(HTTP_SERVICE_CONFIG_URLACL_SET)));
            Marshal.StructureToPtr(inputConfigInfoSet, pInputConfigInfo, false);

            retVal = PInvoke.HttpSetServiceConfiguration(IntPtr.Zero,
                HTTP_SERVICE_CONFIG_ID.HttpServiceConfigUrlAclInfo,
                pInputConfigInfo,
                Marshal.SizeOf(inputConfigInfoSet),
                IntPtr.Zero);

            Marshal.FreeCoTaskMem(pInputConfigInfo);
            PInvoke.HttpTerminate(PInvoke.HTTP_INITIALIZE_CONFIG, IntPtr.Zero);

            if (PInvoke.NO_ERROR != retVal)
            {
                // Error Codes:

                // 5 - ERROR_ACCESS_DENIED
                // 183 - ERROR_ALREADY_EXISTS
                throw new Exception(string.Format("Could not set the Url Acl.  Code {0}", retVal));
            }

            _hasUrlAcl = true;
        }

        private void RemoveUrlAcl()
        {
            var retVal = PInvoke.HttpInitialize(new HTTPAPI_VERSION(2, 0), PInvoke.HTTP_INITIALIZE_CONFIG, IntPtr.Zero);

            if ((uint)PInvoke.NO_ERROR != retVal)
            {
                throw new Exception(string.Format("Could not set the initialize the HTTP API.  Code {0}", retVal));
            }

            var url = string.Format("http://{0}.ngrok.com:{1}/", this.Subdomain, this.LocalhostPort);

            HTTP_SERVICE_CONFIG_URLACL_KEY keyDesc = new HTTP_SERVICE_CONFIG_URLACL_KEY(url);
            HTTP_SERVICE_CONFIG_URLACL_PARAM paramDesc = new HTTP_SERVICE_CONFIG_URLACL_PARAM("D:(A;;GX;;;WD)");

            HTTP_SERVICE_CONFIG_URLACL_SET inputConfigInfoSet = new HTTP_SERVICE_CONFIG_URLACL_SET();
            inputConfigInfoSet.KeyDesc = keyDesc;
            inputConfigInfoSet.ParamDesc = paramDesc;

            IntPtr pInputConfigInfo = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(HTTP_SERVICE_CONFIG_URLACL_SET)));
            Marshal.StructureToPtr(inputConfigInfoSet, pInputConfigInfo, false);

            retVal = PInvoke.HttpDeleteServiceConfiguration(IntPtr.Zero,
                HTTP_SERVICE_CONFIG_ID.HttpServiceConfigUrlAclInfo,
                pInputConfigInfo,
                Marshal.SizeOf(inputConfigInfoSet),
                IntPtr.Zero);

            if (PInvoke.NO_ERROR != retVal)
            {
                throw new Exception(string.Format("Could not remove the Url Acl.  Code {0}", retVal));
            }

            Marshal.FreeCoTaskMem(pInputConfigInfo);
            PInvoke.HttpTerminate(PInvoke.HTTP_INITIALIZE_CONFIG, IntPtr.Zero);

            _hasUrlAcl = false;
        }

        public string Error
        {
            get { return null; }
        }

        public string this[string columnName]
        {
            get
            {
                if (columnName == "Subdomain")
                {
                    if (!IsSubdomainValid) return "Subdomain must be provided in order to configure.";
                }

                return null;
            }

        }

        private bool IsSubdomainValid
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(this.Subdomain))
                {
                    return true;
                }

                return false;
            }
        }

    }
}

using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ngrok.editor.desktop.wpf.Model
{
    public class site : ViewModelBase
    {
        [XmlAttribute()]
        public string name { get; set; }

        [XmlAttribute]
        public string id { get; set; }

        [XmlAttribute]
        public bool serverAutoStart { get; set; }

        [XmlElement]
        public application application { get; set; }

        [XmlArray]
        [XmlArrayItem("binding", typeof(binding))]
        public List<binding> bindings { get; set; }

        //[XmlAttribute]
        //public string subdomain { get; set; }

    }
}

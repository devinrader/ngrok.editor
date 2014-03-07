using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ngrok.editor.desktop.wpf.Model
{
    public class application
    {
        [XmlAttribute]
        public string path { get; set; }

        [XmlAttribute]
        public string applicationPool { get; set; }
        [XmlElement]
        public virtualDirectory virtualDirectory { get; set; }
    }
}

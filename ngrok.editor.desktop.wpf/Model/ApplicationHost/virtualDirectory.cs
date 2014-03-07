using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ngrok.editor.desktop.wpf.Model
{
    public class virtualDirectory
    {
        [XmlAttribute]
        public string path { get; set; }

        [XmlAttribute]
        public string physicalPath { get; set; }
    }
}

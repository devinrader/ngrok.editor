using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ngrok.editor.desktop.wpf.Model
{
    public class binding
    {
        [XmlAttribute]
        public string protocol { get; set; }
        [XmlAttribute]
        public string bindingInformation { get; set; }

        public string ip { get { return parseBindingInformation()[0]; } }
        public string port { get { return parseBindingInformation()[1]; } }
        public string host { get { return parseBindingInformation()[2]; } }


        private string[] parseBindingInformation()
        {
            var parts = this.bindingInformation.Split(':');

            return parts;
        }

        public override string ToString()
        {
            return bindingInformation;
        }
    }
}

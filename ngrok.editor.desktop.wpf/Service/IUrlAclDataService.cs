using ngrok.editor.desktop.wpf.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ngrok.editor.desktop.wpf
{
    public interface IUrlAclDataService
    {
        List<UrlAcl> UrlAcls { get; set; } 
    }
}

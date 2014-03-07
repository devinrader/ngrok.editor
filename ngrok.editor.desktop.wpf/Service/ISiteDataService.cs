using ngrok.editor.desktop.wpf.Model;
using ngrok.editor.desktop.wpf.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ngrok.editor.desktop.wpf
{
    public interface ISiteDataService : INotifyPropertyChanged
    {
        ObservableCollection<SiteViewModel> Sites {get;}

        void Save();
    }
}

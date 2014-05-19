using ngrok.editor.desktop.wpf.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ngrok.editor.desktop.wpf
{
    public interface IConfigurationService : INotifyPropertyChanged
    {
        ApplicationConfiguration ApplicationConfiguration { get; }

        string ApplicationHostConfigPath { get; }

        bool IsConfigurationNeeded { get; }

        bool IsConfigurationValid { get; }

        bool IsForced { get; }

        void Save();

        void ForceConfiguration();
    }
}

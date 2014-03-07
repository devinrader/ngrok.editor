using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ngrok.editor.desktop.wpf
{
    /// <summary>
    /// Interaction logic for ApplicationConfigurationPage.xaml
    /// </summary>
    public partial class ApplicationConfigurationPage : UserControl
    {
        public ApplicationConfigurationPage()
        {
            Messenger.Default.Register<FolderDialogMessage>(this, ShowFolderBrowserDialog);

            InitializeComponent();
        }

        public void ShowFolderBrowserDialog(FolderDialogMessage msg)
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = dialog.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                msg.Callback(dialog.SelectedPath);
            }
        }
    }
}

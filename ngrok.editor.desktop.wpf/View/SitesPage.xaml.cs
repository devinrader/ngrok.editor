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
    /// Interaction logic for SitesPage.xaml
    /// </summary>
    public partial class SitesPage : UserControl
    {
        public SitesPage()
        {
            Messenger.Default.Register<DialogMessage>(this, ShowDialog);

            InitializeComponent();
        }

        public void ShowDialog(DialogMessage msg)
        {
            var result = System.Windows.Forms.MessageBox.Show(msg.Caption);

            if (result == System.Windows.Forms.DialogResult.OK)
            {
//                msg.Callback(dialog.SelectedPath);
            }
        }
    }
}

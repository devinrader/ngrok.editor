using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ngrok.editor.desktop.wpf
{
    public class FolderDialogMessage
    {
        public FolderDialogMessage(Action<string> callback)
        {
            this.Callback = callback;
        }

        public Action<string> Callback { get; set; }
    }
}

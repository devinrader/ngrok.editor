using GalaSoft.MvvmLight;
using ngrok.editor.desktop.wpf.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ngrok.editor.desktop.wpf.ViewModel
{
    public class BindingViewModel : ViewModelBase
    {
        binding _binding;

        public BindingViewModel(binding binding)
        {
            _binding = binding;
        }

        public string protocol { get { return _binding.protocol; } }
        public string bindingInformation { get { return _binding.bindingInformation; } }


        public string Ip { get { return parseBindingInformation()[0]; } }
        public string Port { get { return parseBindingInformation()[1]; } }
        public string Host { get { return parseBindingInformation()[2]; } }

        public string Subdomain
        {
            get 
            {
                var parts = this.Host.Split('.');
                if (parts.Length >=3)
                    return parts[0];

                return String.Empty;
            } 
        }

        private string[] parseBindingInformation()
        {
            var parts = _binding.bindingInformation.Split(':');

            return parts;
        }
    }
}

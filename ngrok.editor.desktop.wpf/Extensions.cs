using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ngrok.editor.desktop.wpf
{
    public static class Extensions
    {
        public static string GetValueOrDefault(this KeyValueConfigurationCollection settings, string key)
        {
            if (settings.AllKeys.Contains(key))
                return settings[key].Value;

            return string.Empty;
        }

        public static void CreateOrUpdateKey(this KeyValueConfigurationCollection settings, string key, string value)
        {
            if (settings.AllKeys.Contains(key))
            {
                settings[key].Value = value;
            }
            else
            {
                settings.Add(key, value);
            }
        }
    }
}

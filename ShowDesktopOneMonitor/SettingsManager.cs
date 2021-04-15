using ShowDesktopOneMonitor.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShowDesktopOneMonitor
{
    /// <summary>
    /// Util class for reading and writing to settings file
    /// </summary>
    class SettingsManager
    {
        private static Settings SETTINGS = Properties.Settings.Default;

        public static Keys ReadHotkey ()
        {
            return SETTINGS.HotKey;
        }
        public static KeyModifiers ReadKeyModifiers ()
        {
            KeyModifiers modifiers = SETTINGS.KeyModifiers[0];
            Array.ForEach(SETTINGS.KeyModifiers, m => modifiers |= m);
            return modifiers;
        }

        public static void WriteSave (Action action)
        {
            action();
            SETTINGS.Save();
        }
    }
}

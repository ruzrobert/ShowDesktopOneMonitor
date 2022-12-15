using ShowDesktopOneMonitor.Properties;
using System;
using System.Windows.Forms;

namespace ShowDesktopOneMonitor
{
    /// <summary>
    /// Util class for reading and writing to settings file
    /// </summary>
    static class SettingsManager
    {
        private static readonly Settings SETTINGS = Settings.Default;

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

        public static void Save () => SETTINGS.Save();

    }
}

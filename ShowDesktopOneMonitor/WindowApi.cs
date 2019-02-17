using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ShowDesktopOneMonitor
{
    public class WindowApi
    {
        [DllImport("user32.dll")]
        private static extern IntPtr GetWindow (IntPtr hWnd, uint uCmd);

        public static int GetWindowZOrder (IntPtr handle)
        {
            const uint GW_HWNDPREV = 3;
            const uint GW_HWNDLAST = 1;

            var lowestHwnd = GetWindow(handle, GW_HWNDLAST);
            int zOrder = 0;

            var z = 0;
            var hwndTmp = lowestHwnd;
            while (hwndTmp != IntPtr.Zero) {
                if (handle == hwndTmp) {
                    zOrder = z;
                    return zOrder;
                }

                hwndTmp = GetWindow(hwndTmp, GW_HWNDPREV);
                z++;
            }

            zOrder = int.MinValue;
            return -1;
        }
    }
}
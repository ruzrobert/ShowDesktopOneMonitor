using System.Drawing;
using System.Runtime.InteropServices;

namespace FrigoTab {

    public static class Points {

        public static Point ClientToScreen (this Point point, WindowHandle handle) {
            ClientToScreen(handle, ref point);
            return point;
        }

        public static Point ScreenToClient (this Point point, WindowHandle handle) {
            ScreenToClient(handle, ref point);
            return point;
        }

        [DllImport("user32.dll")]
        private static extern bool ClientToScreen (WindowHandle hWnd, ref Point lpPoint);

        [DllImport("user32.dll")]
        private static extern bool ScreenToClient (WindowHandle hWnd, ref Point lpPoint);

    }

}

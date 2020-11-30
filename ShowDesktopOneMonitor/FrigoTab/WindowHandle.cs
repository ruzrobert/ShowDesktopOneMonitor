using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace FrigoTab {

    [Flags]
    public enum WindowStyles : long {

        Disabled = 0x8000000,
        Visible = 0x10000000,
        Minimize = 0x20000000

    }

    [Flags]
    public enum WindowExStyles : long {

        Transparent = 0x20,
        ToolWindow = 0x80,
        AppWindow = 0x40000,
        Layered = 0x80000,
        NoActivate = 0x8000000

    }

    public struct WindowHandle {

        public static readonly WindowHandle Null = new WindowHandle(IntPtr.Zero);
        public static bool operator == (WindowHandle h1, WindowHandle h2) => h1.handle == h2.handle;
        public static bool operator != (WindowHandle h1, WindowHandle h2) => h1.handle != h2.handle;

        [DllImport("user32.dll")]
        public static extern WindowHandle GetForegroundWindow ();

        private readonly IntPtr handle;

        public WindowHandle (IntPtr handle) => this.handle = handle;
        public override bool Equals (object obj) => obj != null && GetType() == obj.GetType() && handle == ((WindowHandle) obj).handle;
        public override int GetHashCode () => handle.GetHashCode();
        public Screen GetScreen () => Screen.FromHandle(handle);
        public WindowStyles GetWindowStyles () => (WindowStyles) GetWindowLongPtr(this, WindowLong.Style);
        public WindowExStyles GetWindowExStyles () => (WindowExStyles) GetWindowLongPtr(this, WindowLong.ExStyle);
        public void PostMessage (WindowMessages msg, int wParam, int lParam) => PostMessage(this, msg, (IntPtr) wParam, (IntPtr) lParam);
        public IntPtr GetHandle () => this.handle;

        public void SetForeground () {
            if( GetWindowStyles().HasFlag(WindowStyles.Minimize) ) {
                ShowWindow(this, ShowWindowCommand.Restore);
            }
            keybd_event(0, 0, 0, 0);
            SetForegroundWindow(this);
        }
        public void SetMinimizeWindow ()
        {
            if (GetWindowStyles().HasFlag(WindowStyles.Visible)) {
                ShowWindow(this, ShowWindowCommand.ShowMinimized);
            }
        }
        public void SetRestoreWindow ()
        {
            if (GetWindowStyles().HasFlag(WindowStyles.Minimize)) {
                ShowWindow(this, ShowWindowCommand.Restore);
            }
        }

        public string GetWindowText () {
            StringBuilder text = new StringBuilder(GetWindowTextLength(this) + 1);
            GetWindowText(this, text, text.Capacity);
            return text.ToString();
        }

        public Rect GetRect () {
            WindowPlacement placement = GetWindowPlacement();
            switch( placement.ShowCmd ) {
                case ShowWindowCommand.ShowNormal:
                case ShowWindowCommand.ShowMinimized:
                    return placement.NormalPosition;
                case ShowWindowCommand.ShowMaximized:
                    GetWindowRect(this, out Rect rect);
                    return rect;
                default:
                    throw new ArgumentException();
            }
        }

        private WindowPlacement GetWindowPlacement () {
            WindowPlacement placement = new WindowPlacement();
            placement.Length = Marshal.SizeOf(placement);
            GetWindowPlacement(this, ref placement);
            return placement;
        }

        private struct WindowPlacement {

            public int Length;
            public int Flags;
            public ShowWindowCommand ShowCmd;
            public Point MinPosition;
            public Point MaxPosition;
            public Rect NormalPosition;

        }

        private enum ShowWindowCommand {

            ShowNormal = 1,
            ShowMinimized = 2,
            ShowMaximized = 3,
            Restore = 9

        }

        private enum WindowLong {

            ExStyle = -20,
            Style = -16

        }

        // Win32 does not support GetWindowLongPtr directly
        private static IntPtr GetWindowLongPtr(WindowHandle hWnd, WindowLong nIndex)
        {
            if (IntPtr.Size == 8)
            {
                return GetWindowLongPtr64(hWnd, nIndex);
            }
            else
            {
                return GetWindowLongPtr32(hWnd, nIndex);
            }
        }

        [DllImport("user32.dll")]
        private static extern int GetWindowTextLength (WindowHandle hWnd);

        [DllImport("user32.dll")]
        private static extern int GetWindowText (WindowHandle hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
        private static extern IntPtr GetWindowLongPtr32 (WindowHandle hWnd, WindowLong nIndex);

        [DllImport("user32.dll", EntryPoint = "GetWindowLongPtr")]
        private static extern IntPtr GetWindowLongPtr64(WindowHandle hWnd, WindowLong nIndex);

        [DllImport("user32.dll")]
        private static extern bool ShowWindow (WindowHandle hWnd, ShowWindowCommand nCmdShow);

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow (WindowHandle hWnd);

        [DllImport("user32.dll")]
        private static extern void keybd_event (byte bVk, byte bScan, int dwFlags, int dwExtraInfo);

        [DllImport("user32.dll")]
        private static extern bool PostMessage (WindowHandle hWnd, WindowMessages msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern bool GetWindowRect (WindowHandle hWnd, out Rect rect);

        [DllImport("user32.dll")]
        private static extern bool GetWindowPlacement (WindowHandle hWnd, ref WindowPlacement lpwndpl);

    }

}

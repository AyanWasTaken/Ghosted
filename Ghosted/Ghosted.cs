using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Ghosted
{
    public class Main
    {
        [DllImport("user32.dll")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        private static extern bool SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll")]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        private const int SW_HIDE = 0;
        private const int SW_SHOW = 5;
        private const int GWL_EXSTYLE = -20;
        private const int WS_EX_TOOLWINDOW = 0x00000080;
        private const int WS_EX_APPWINDOW = 0x00040000;

        private static IntPtr GetMainWindowHandle()
        {
            return Process.GetCurrentProcess().MainWindowHandle;
        }

        public static void Ghost()
        {
            IntPtr handle = GetMainWindowHandle();

            if (handle != IntPtr.Zero)
            {
                // Update 1.1: Hide from Alt+Tab (by setting ToolWindow style and removing AppWindow)
                int style = GetWindowLong(handle, GWL_EXSTYLE);
                SetWindowLong(handle, GWL_EXSTYLE, (style | WS_EX_TOOLWINDOW) & ~WS_EX_APPWINDOW);

                ShowWindow(handle, SW_HIDE);
            }
        }

        public static void Unghost()
        {
            IntPtr handle = GetMainWindowHandle();

            if (handle != IntPtr.Zero)
            {
                int style = GetWindowLong(handle, GWL_EXSTYLE);
                SetWindowLong(handle, GWL_EXSTYLE, (style | WS_EX_APPWINDOW) & ~WS_EX_TOOLWINDOW);

                ShowWindow(handle, SW_SHOW);
            }
        }
    }
}

using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.WindowsAndMessaging;
using Windows.Win32.System.Threading;
namespace WinWrapper
{
    public class Taskbar
    {
        static Lazy<HWND> _TaskBarHWND =
            new(() => PInvoke.FindWindow("Shell_TrayWnd", null));
        public static HWND TaskBarHWND => _TaskBarHWND.Value;
        static Lazy<HWND> _MultiTaskBarHWND =
            new(() => PInvoke.FindWindow("Shell_SecondaryTrayWnd", null));
        public static HWND MultiTaskBarHWND => _MultiTaskBarHWND.Value;
        static Lazy<HWND> _StartMenuHWND =
            new(delegate
            {
                var a = PInvoke.FindWindowEx(default, default, "Button", "Start");
                if (a == default)
                    a = PInvoke.FindWindow("Button", null);
                return a;
            });
        public static HWND StartMenuHWND => _StartMenuHWND.Value;

        public enum WindowZOrder
        {
            HWND_TOP = 0,
            HWND_BOTTOM = 1,
            HWND_TOPMOST = -1,
            HWND_NOTOPMOST = -2,
        }

        public static void ShowTaskbar() => SetVisibility(true);

        public static void HideTaskbar() => SetVisibility(false);

        private static void SetVisibility(bool isVisible)
        {

            if (!isVisible)
            {
                PInvoke.SetWindowPos(
                    TaskBarHWND,
                    new((IntPtr)WindowZOrder.HWND_BOTTOM),
                    0, 0, 0, 0,
                    SET_WINDOW_POS_FLAGS.SWP_HIDEWINDOW |
                    SET_WINDOW_POS_FLAGS.SWP_NOMOVE |
                    SET_WINDOW_POS_FLAGS.SWP_NOSIZE |
                    SET_WINDOW_POS_FLAGS.SWP_NOACTIVATE
                );
            }
            else
            {
                PInvoke.SetWindowPos(
                    TaskBarHWND,
                    new((IntPtr)WindowZOrder.HWND_TOPMOST),
                    0, 48, 0, 0,
                    SET_WINDOW_POS_FLAGS.SWP_SHOWWINDOW
                );
            }
        }
        public static void ToggleStart()
        {
            PInvoke.SendMessage(TaskBarHWND, /*WM_SYSCOMMAND*/ 0x0112, /*SC_TASKLIST*/ 0xF130, 0);
        }
        private const int ID_TRAY_SHOW_OVERFLOW = 0x028a;
        private const int ID_TRAY_HIDE_OVERFLOW = 0x028b;
        public static void ShowSysTray()
        {
            PInvoke.SendMessage(TaskBarHWND, /*WM_COMMAND*/ 0x0111, ID_TRAY_SHOW_OVERFLOW, IntPtr.Zero);
        }

        /// <summary>
        /// Wifi, Sound, and Battery
        /// </summary>
        public static void ToggleSysControl()
        {
            PInvoke.ShellExecute(default,
                "open",
                "ms-actioncenter:controlcenter/&suppressAnimations=false&showFooter=true&allowPageNavigation=true",
                null,
                null,
                SHOW_WINDOW_CMD.SW_SHOWNORMAL
            );
        }

        public static void ToggleActionCenter()
        {
            PInvoke.ShellExecute(default,
                "open",
                "ms-actioncenter:",
                null,
                null,
                SHOW_WINDOW_CMD.SW_SHOWNORMAL
            );
        }

        public static void SendWinlogonShowShell()
        {
            var handle = PInvoke.OpenEvent(SYNCHRONIZATION_ACCESS_RIGHTS.EVENT_MODIFY_STATE, false, "msgina: ShellReadyEvent");
            if (!handle.IsInvalid)
            {
                PInvoke.SetEvent(handle);
                handle.Dispose();
            }
        }
    }
}

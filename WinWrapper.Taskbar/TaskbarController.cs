using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.WindowsAndMessaging;
using Windows.Win32.System.Threading;
using WinWrapper.Windowing;
namespace WinWrapper.Taskbar;

public class TaskbarController
{
    static Lazy<Window> _TaskBarHWND =
        new(() => Window.Find(WindowClass.FromExistingClass("Shell_TrayWnd"), null));
    public static Window TaskBarWindow => _TaskBarHWND.Value;
    static Lazy<Window> _MultiTaskBarHWND =
        new(() => Window.Find(WindowClass.FromExistingClass("Shell_SecondaryTrayWnd"), null));
    public static Window MultiTaskBarHWND => _MultiTaskBarHWND.Value;
    static Lazy<Window> _StartMenuHWND =
        new(delegate
        {
            var a = Window.Find(default, default, WindowClass.FromExistingClass("Button"), "Start");
            if (a == default)
                a = Window.Find(WindowClass.FromExistingClass("Button"), null);
            return a;
        });
    public static Window StartMenuHWND => _StartMenuHWND.Value;

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
            TaskBarWindow.SetVisibility(false);
        }
        else
        {
            var TaskBarWindow = TaskbarController.TaskBarWindow;
            TaskBarWindow.SetVisibility(true);
            TaskBarWindow.Bounds = new(0, 48, 0, 0);
            //PInvoke.SetWindowPos(
            //    TaskBarHWND,
            //    new((IntPtr)WindowZOrder.HWND_TOPMOST),
            //    0, 48, 0, 0,
            //    SET_WINDOW_POS_FLAGS.SWP_SHOWWINDOW
            //);
        }
    }
    public static void ToggleStart()
    {
        TaskBarWindow.SendMessage(WindowMessages.SysCommand, /*SC_TASKLIST*/ 0xF130, 0);
    }
    private const int ID_TRAY_SHOW_OVERFLOW = 0x028a;
    private const int ID_TRAY_HIDE_OVERFLOW = 0x028b;
    public static void ShowSysTray()
    {
        TaskBarWindow.SendMessage(WindowMessages.Command, ID_TRAY_SHOW_OVERFLOW, IntPtr.Zero);
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

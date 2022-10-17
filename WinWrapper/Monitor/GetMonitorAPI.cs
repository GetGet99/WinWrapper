using Windows.Win32;
using Windows.Win32.Foundation;
namespace WinWrapper;

static class GetWindowAPI
{
    private static List<Window> _WinStructList = new();

    private static BOOL Callback(HWND hWnd, LPARAM lparam)
    {
        _WinStructList.Add(Window.FromWindowHandle((IntPtr)hWnd));
        return true;
    }

    public static List<Window> EnumWindows()
    {
        _WinStructList = new List<Window>();
        PInvoke.EnumWindows(Callback, IntPtr.Zero);
        return _WinStructList;
    }
    public static List<Window> EnumChildWindows(HWND hWndParent)
    {
        _WinStructList = new List<Window>();
        PInvoke.EnumChildWindows(hWndParent, Callback, 0);
        return _WinStructList;
    }
    public static List<Window> EnumCurrentThreadWindows()
    {
        _WinStructList = new List<Window>();
        PInvoke.EnumThreadWindows((uint)PInvoke.GetCurrentThread().Value, Callback, 0);
        return _WinStructList;
    }
    public static List<Window> EnumThreadWindows(uint ThreadId)
    {
        _WinStructList = new List<Window>();
        PInvoke.EnumThreadWindows(ThreadId, Callback, 0);
        return _WinStructList;
    }
    public unsafe static List<Window> EnumSameThreadWindows(Window Window)
    {
        _WinStructList = new List<Window>();
        PInvoke.EnumThreadWindows(PInvoke.GetWindowThreadProcessId(Window.Handle, (uint*)0), Callback, 0);
        return _WinStructList;
    }
}
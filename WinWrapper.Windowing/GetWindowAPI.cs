using Windows.Win32;
using Windows.Win32.Foundation;
using WinWrapper.Windowing;
namespace WinWrapper.Windowing;

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
    public static List<Window> EnumThreadWindows(Thread ThreadId)
    {
        _WinStructList = new List<Window>();
        PInvoke.EnumThreadWindows((uint)ThreadId.Handle, Callback, 0);
        return _WinStructList;
    }
}
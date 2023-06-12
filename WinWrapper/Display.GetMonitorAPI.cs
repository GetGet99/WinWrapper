using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.Graphics.Gdi;

namespace WinWrapper;

static class GetMonitorAPI
{
    private static List<Display> _WinStructList = new();

    private unsafe static BOOL Callback(HMONITOR Handle, HDC DeviceContext, RECT* Region, LPARAM lparam)
    {
        _WinStructList.Add(Display.FromHandle(Handle));
        return true;
    }

    public unsafe static List<Display> EnumMonitors()
    {
        _WinStructList = new List<Display>();
        PInvoke.EnumDisplayMonitors(default, default(RECT*), Callback, 0);
        return _WinStructList;
    }
}
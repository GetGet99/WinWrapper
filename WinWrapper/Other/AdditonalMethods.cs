using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using Windows.Win32.Foundation;
using Windows.Win32.Graphics.Gdi;
using Windows.Win32.UI.Shell;
using Windows.Win32.UI.WindowsAndMessaging;

namespace Windows.Win32;
#pragma warning disable CA1401
partial class PInvoke
{
    public const int SC_MOUSEMOVE = 0xf012;
    public const int SC_MOUSEMENU = 0xf090;

    [DllImport("Oleacc.dll", ExactSpelling = true)]
    public static extern HANDLE GetProcessHandleFromHwnd(HWND hwnd);

    [DllImport("gdiplus.dll", ExactSpelling = true)]
    public static extern int GdipCreateBitmapFromHICON(HICON hicon, out HBITMAP bitmap);
}

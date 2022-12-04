using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Win32;
using Windows.Win32.UI.WindowsAndMessaging;
using Windows.Win32.Foundation;
using Windows.Win32.Graphics.Gdi;
namespace WinWrapper;

public readonly struct WindowClass
{
    readonly WNDPROC? WndProc = default;
    public readonly string ClassName;
    private WindowClass(string ClassName, bool _) { this.ClassName = ClassName; }
    public unsafe WindowClass(string ClassName, WNDPROC? WndProc = null, WNDCLASS_STYLES ClassStyle = default, HBRUSH BackgroundBrush = default)
    {
        this.ClassName = ClassName;
        WndProc ??= (hwnd, msg, wParam, lParam) => PInvoke.DefWindowProc(hwnd, msg, wParam, lParam);
        this.WndProc = WndProc;
        var hInstance = PInvoke.GetCurrentProcess();
        fixed (char* className = ClassName)
        {
            WNDCLASSW cls = new()
            {
                lpszClassName = className,
                hInstance = new HINSTANCE(hInstance.Value),
                lpfnWndProc = WndProc,
                style = ClassStyle,
                hbrBackground = BackgroundBrush
            };
            PInvoke.RegisterClass(cls);
        }
    }

    public static WindowClass FromExistingClass(string ClassName) => new(ClassName, false);
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Win32;
using Windows.Win32.UI.WindowsAndMessaging;
using Windows.Win32.Foundation;
namespace WinWrapper;

public readonly struct WindowClass
{
    public readonly string ClassName;
    private WindowClass(string ClassName, bool _) { this.ClassName = ClassName; }
    public unsafe WindowClass(string ClassName)
        : this(ClassName, (hwnd, msg, wParam, lParam) => PInvoke.DefWindowProc(hwnd, msg, wParam, lParam))
    { }

    public unsafe WindowClass(string ClassName, WNDPROC WndProc)
    {
        this.ClassName = ClassName;
        var hInstance = PInvoke.GetCurrentProcess();
        fixed (char* className = ClassName)
        {
            WNDCLASSW cls = new()
            {
                lpszClassName = className,
                hInstance = new HINSTANCE(hInstance.Value),
                lpfnWndProc = WndProc
            };
            PInvoke.RegisterClass(cls);
        }
    }

    public static WindowClass FromExistingClass(string ClassName) => new(ClassName, false);
}

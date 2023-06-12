using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Win32;
using Windows.Win32.UI.WindowsAndMessaging;
using Windows.Win32.Foundation;
using Windows.Win32.Graphics.Gdi;
namespace WinWrapper.Windowing;

public readonly struct WindowClass
{
    public unsafe delegate nint WindowProc(Window hWnd, uint WindowMessage, nuint wParam, nint lParam);
    readonly WNDPROC? WndProc = default;
    public readonly string ClassName;
    private WindowClass(string ClassName, bool _) { this.ClassName = ClassName; }
    internal unsafe WindowClass(
        string ClassName,
        WindowProc? WndProc = null,
        WindowClassStyles ClassStyle = default,
        HBRUSH BackgroundBrush = default
    )
    {
        this.ClassName = ClassName;
        if (WndProc != null)
            this.WndProc = (hWnd, WM, WP, LP) => new(WndProc.Invoke(
                Window.FromWindowHandle(hWnd),
                WM,
                WP,
                LP
            ));
        this.WndProc ??= PInvoke.DefWindowProc;
        var hInstance = Process.Current;
        fixed (char* className = ClassName)
        {
            WNDCLASSW cls = new()
            {
                lpszClassName = className,
                hInstance = new HINSTANCE((nint)hInstance.Id),
                lpfnWndProc = this.WndProc,
                style = (WNDCLASS_STYLES)ClassStyle,
                hbrBackground = BackgroundBrush
            };
            PInvoke.RegisterClass(cls);
        }
    }
    public static WindowClass FromExistingClass(string ClassName) => new(ClassName, false);
}

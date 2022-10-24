using System.Drawing;
using System.Runtime.CompilerServices;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.WindowsAndMessaging;
namespace WinWrapper;

partial struct Window : IEquatable<Window>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Window FromLocation(int X, int Y) => FromLocation(new Point(X, Y));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Window FromLocation(Point pt)
        => new(PInvoke.WindowFromPoint(pt));
    /// <summary>
    /// Gets Acitve <see cref="Window"/> of the current Thread
    /// </summary>
    public static Window ActiveWindow
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(PInvoke.GetActiveWindow());
    }

    /// <summary>
    /// Gets Foreground <see cref="Window"/>
    /// </summary>
    public static Window ForegroundWindow
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(PInvoke.GetForegroundWindow());
    }

    public static Window CreateNewWindow(string Title, Rectangle Bounds = default)
    {
        if (Bounds == default)
        {
            const int d = PInvoke.CW_USEDEFAULT;
            Bounds = new Rectangle(d, d, d, d);
        }
        var hInstance = PInvoke.GetCurrentProcess();
        unsafe
        {
            fixed (char* className = "ME")
            {
                WNDCLASSW cls = new()
                {
                    lpszClassName = className,
                    hInstance = new HINSTANCE(hInstance.Value),
                    lpfnWndProc = (hwnd, msg, wParam, lParam) =>
                    {
                        return PInvoke.DefWindowProc(hwnd, msg, wParam, lParam);
                    }
                };
                PInvoke.RegisterClass(cls);
                return new(PInvoke.CreateWindowEx(
                    WINDOW_EX_STYLE.WS_EX_OVERLAPPEDWINDOW,
                    "ME",
                    Title,
                    WINDOW_STYLE.WS_OVERLAPPEDWINDOW,
                    Bounds.X,
                    Bounds.Y,
                    Bounds.Width,
                    Bounds.Height,
                    HWND.Null,
                    null,
                    null,
                    (void*)IntPtr.Zero
                ));
            }
        }
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static List<Window> GetAllWindows()
        => GetWindowAPI.EnumWindows();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static List<Window> GetWindowsInCurrentThread()
        => GetWindowAPI.EnumCurrentThreadWindows();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static List<Window> GetWindowsInThread(uint ThreadId)
        => GetWindowAPI.EnumThreadWindows(ThreadId);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static List<Window> GetSameThreadWindows(Window Window)
        => GetWindowAPI.EnumSameThreadWindows(Window);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Window GetWindowFromPoint(Point pt)
        => new(PInvoke.WindowFromPoint(pt));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Window FromWindowHandle(IntPtr Handle)
        => new((HWND)Handle);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Window FromWindowHandle(HWND Handle)
        => new(Handle);

}

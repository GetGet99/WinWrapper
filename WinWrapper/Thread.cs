using Windows.Win32;
using Windows.Win32.Foundation;

namespace WinWrapper;

public readonly struct Thread
{
    private Thread(HANDLE handle) => HANDLE = handle;
    internal readonly HANDLE HANDLE;
    public readonly nint Handle => HANDLE;
    public static Thread Current => FromHandle(PInvoke.GetCurrentThread());

    public static Thread FromHandle(IntPtr handle) => FromHandle(new HANDLE(handle));
    internal static Thread FromHandle(HANDLE handle) => new(handle);
}

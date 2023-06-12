//// Copy and paste from https://github.com/dotMorten/WinUIEx/blob/main/src/WinUIEx/Messaging/WindowMessageMonitor.cs
//// With Minor Modifications
//using Microsoft.Win32;
//using Windows.Win32.Foundation;
//using Windows.Win32.UI.WindowsAndMessaging;

//namespace WinWrapper;

//public class WindowMessageMonitor : IDisposable
//{
//    private Window Window;
//    private delegate IntPtr WinProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);
//    private HOOKPROC? callback;
//    private readonly object _lockObject = new object();

//    /// <summary>
//    /// Initialize a new instance of the <see cref="WindowMessageMonitor"/> class.
//    /// </summary>
//    /// <param name="hwnd">The window handle to listen to messages for</param>
//    public WindowMessageMonitor(Window window)
//    {
//        Window = window;
//    }

//    /// <summary>
//    /// Finalizer
//    /// </summary>
//    ~WindowMessageMonitor()
//    {
//        Dispose(false);
//    }

//    /// <summary>
//    /// Disposes this instance
//    /// </summary>
//    public void Dispose()
//    {
//        Dispose(true);
//        GC.SuppressFinalize(this);
//    }

//    private void Dispose(bool disposing)
//    {
//        if (_NativeMessage != null)
//            Unsubscribe();
//    }

//    public delegate LRESULT? WindowMessageCallback(Window window, int uMsg, WPARAM wParam, LPARAM lParam);

//    private event WindowMessageCallback? _NativeMessage;

//    /// <summary>
//    /// Event raised when a windows message is received.
//    /// </summary>
//    public event WindowMessageCallback WindowMessageReceived
//    {
//        add
//        {
//            if (_NativeMessage is null)
//            {
//                Subscribe();
//            }
//            _NativeMessage += value;
//        }
//        remove
//        {
//            _NativeMessage -= value;
//            if (_NativeMessage is null)
//            {
//                Unsubscribe();
//            }
//        }
//    }

//    private LRESULT NewWindowProc(int uMsg, WPARAM wParam, LPARAM lParam)
//    {
//        var handler = _NativeMessage;
//        if (handler != null)
//        {
//            var result = handler.Invoke(Window, uMsg, wParam.Value, lParam);
//            if (result.HasValue)
//                return result.Value;
//        }
//        return Windows.Win32.PInvoke.CallNextHookEx(, uMsg, wParam, lParam);
//    }

//    private void Subscribe()
//    {
//        lock (_lockObject)
//            if (callback == null)
//            {
//                callback = new HOOKPROC(NewWindowProc);
//                bool ok = Windows.Win32.PInvoke.SetWindowsHookEx(
//                    WINDOWS_HOOK_ID.WH_CALLWNDPROC,
//                    callback,
//                    Window,
//                    Window.
//                );
//            }
//    }

//    private void Unsubscribe()
//    {
//        lock (_lockObject)
//            if (callback != null)
//            {
//                Windows.Win32.PInvoke.RemoveWindowSubclass(Window, callback, 101);
//                callback = null;
//            }
//    }
//}
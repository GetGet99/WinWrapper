using Windows.Win32;
using Windows.Win32.UI.WindowsAndMessaging;
namespace WinWrapper;

public class Application
{
    public static unsafe void RunMessageLoopOnCurrentThread()
    {
        var msg = new MSG();
        while (true)
        {
            var res = PInvoke.GetMessage(&msg, default, 0, 0);

            if (res.Value is 0 or -1) break;

            PInvoke.TranslateMessage(&msg);
            PInvoke.DispatchMessage(&msg);
        }
    }
}
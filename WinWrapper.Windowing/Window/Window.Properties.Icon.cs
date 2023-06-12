using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using Windows.Win32;
using Windows.Win32.Foundation;
namespace WinWrapper.Windowing;

partial struct Window
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public nint SmallIconPtr
    {
        get
        {
            var handle = (nint)SendMessage(WindowMessages.GetIcon, PInvoke.ICON_SMALL, default(LPARAM));
            if (handle is 0) handle = (nint)SendMessage(WindowMessages.GetIcon, PInvoke.ICON_SMALL2, default(LPARAM));
            if (handle is 0) handle = (nint)WindowClassLong[WindowClassLongIndex.GCLP_HICONSM];

            return handle;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set
        {
            PInvoke.SendMessage(HWND, PInvoke.WM_SETICON, PInvoke.ICON_SMALL, value);
        }
    }
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public nint LargeIconPtr
    {
        get
        {
            var handle = (nint)SendMessage(WindowMessages.GetIcon, PInvoke.ICON_BIG, default(LPARAM));
            if (handle == 0) handle = (nint)WindowClassLong[WindowClassLongIndex.GCLP_HICON];
            return handle;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set
        {
            PInvoke.SendMessage(HWND, PInvoke.WM_SETICON, PInvoke.ICON_BIG, value);
        }
    }
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Bitmap? SmallIcon
    {
        get
        {
            try
            {
                var handle = SmallIconPtr;
                if (handle is 0) return null;
                return Bitmap.FromHicon(handle);
            }
            catch
            {
                return null;
            }
        }
    }
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Bitmap? LargeIcon
    {
        get
        {
            try
            {
                var handle = LargeIconPtr;
                if (handle is 0) return null;
                return Bitmap.FromHicon(handle);
            }
            catch
            {
                return null;
            }

        }
    }
}
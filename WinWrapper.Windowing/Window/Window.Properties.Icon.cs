using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using Windows.Win32;
using Windows.Win32.Foundation;
namespace WinWrapper.Windowing;

partial struct Window
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Icon SmallIcon
    {
        get
        {
            var handle = (nint)SendMessage(WindowMessages.GetIcon, PInvoke.ICON_SMALL, default(LPARAM));
            if (handle is 0) handle = (nint)SendMessage(WindowMessages.GetIcon, PInvoke.ICON_SMALL2, default(LPARAM));
            if (handle is 0) handle = (nint)WindowClassLong[WindowClassLongIndex.GCLP_HICONSM];

            return Icon.FromHandle(handle);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set
        {
            SendMessage(WindowMessages.SetIcon, PInvoke.ICON_SMALL, value.Handle);
        }
    }
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Icon LargeIcon
    {
        get
        {
            var handle = (nint)SendMessage(WindowMessages.GetIcon, PInvoke.ICON_BIG, default(LPARAM));
            if (handle == 0) handle = (nint)WindowClassLong[WindowClassLongIndex.GCLP_HICON];
            return Icon.FromHandle(handle);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set
        {
            SendMessage(WindowMessages.SetIcon, PInvoke.ICON_BIG, value.Handle);
        }
    }
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Bitmap? SmallIconAsBitmap
    {
        get
        {
            try
            {
                var icon = SmallIcon;
                if (icon.Handle is 0) return null;
                return Bitmap.FromHicon(icon.Handle);
            }
            catch
            {
                return null;
            }
        }
    }
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Bitmap? LargeIconAsBitmap
    {
        get
        {
            try
            {
                var icon = LargeIcon;
                if (icon.Handle is 0) return null;
                return Bitmap.FromHicon(icon.Handle);
            }
            catch
            {
                return null;
            }

        }
    }
}
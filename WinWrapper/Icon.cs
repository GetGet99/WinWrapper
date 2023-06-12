using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Win32;
using Windows.Win32.Graphics.Gdi;
using Windows.Win32.UI.WindowsAndMessaging;

namespace WinWrapper;

public readonly struct Icon : IDisposable
{
    readonly HICON Handle;
    private Icon(HICON IconHandle)
    {
        Handle = IconHandle;
    }

    internal static Icon FromHandle(HICON ProcessHandle) => new(ProcessHandle);
    public static Icon FromHandle(nint ProcessHandle) => new((HICON)ProcessHandle);
    internal ICONINFO Info
    {
        get
        {
            unsafe
            {
                ICONINFO Info = new();
                PInvoke.GetIconInfo(Handle, &Info);
                return Info;
            }
        }
    }
    internal HBITMAP BitmapColor => Info.hbmColor;
    internal HBITMAP BitmapMask => Info.hbmMask;
    internal HBITMAP Bitmap
    {
        get
        {
            PInvoke.GdipCreateBitmapFromHICON(Handle, out var icon);
            return icon;
        }
    }
    public nint HBitmap => Bitmap;

    public System.Drawing.Icon ToSysDrawIcon() => System.Drawing.Icon.FromHandle(Handle);
    public System.Drawing.Bitmap ToSysDrawBitmap() => System.Drawing.Bitmap.FromHicon(Handle);

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}

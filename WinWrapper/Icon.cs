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
    public Icon(HICON IconHandle)
    {
        Handle = IconHandle;
    }
    public System.Drawing.Icon ToSysDrawIcon() => System.Drawing.Icon.FromHandle(Handle);
    public ICONINFO Info
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
    public HBITMAP BitmapColor => Info.hbmColor;
    public HBITMAP BitmapMask => Info.hbmMask;
    public HBITMAP Bitmap
    {
        get
        {
            PInvoke.GdipCreateBitmapFromHICON(Handle, out var icon);
            return icon;
        }
    }
    public System.Drawing.Bitmap ToSysDrawBitmap() => System.Drawing.Bitmap.FromHicon(Handle);

    public void Dispose()
    {
        
        GC.SuppressFinalize(this);
    }
}

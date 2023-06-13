using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.Graphics.Gdi;
using Windows.Win32.UI.WindowsAndMessaging;

namespace WinWrapper;

public readonly struct Icon : IDisposable
{
    internal readonly HICON HICON;
    public nint Handle => HICON;
    private Icon(HICON IconHandle)
    {
        HICON = IconHandle;
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
                PInvoke.GetIconInfo(HICON, &Info);
                return Info;
            }
        }
    }

    public static unsafe Icon Load(string Path)
    {
        fixed (char* c = Path)
        {
            var img = PInvoke.LoadImage(
                hInst: default,
                name: c,
                type: GDI_IMAGE_TYPE.IMAGE_ICON,
                cx: 0,
                cy: 0,
                fuLoad: IMAGE_FLAGS.LR_LOADFROMFILE | IMAGE_FLAGS.LR_DEFAULTSIZE | IMAGE_FLAGS.LR_SHARED
            );
            return new((HICON)img.Value);
        }
    }
    internal HBITMAP BitmapColor => Info.hbmColor;
    internal HBITMAP BitmapMask => Info.hbmMask;
    internal HBITMAP Bitmap
    {
        get
        {
            _ = PInvoke.GdipCreateBitmapFromHICON(HICON, out var icon);
            return icon;
        }
    }
    public nint HBitmap => Bitmap;

    public System.Drawing.Icon ToSysDrawIcon() => System.Drawing.Icon.FromHandle(HICON);
    public System.Drawing.Bitmap ToSysDrawBitmap() => System.Drawing.Bitmap.FromHicon(HICON);

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Returns the readable string representation of <see cref="Window"/>
    /// </summary>
    public override string ToString()
    {
        return $"Icon {HICON.Value}";
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(Icon other)
        => this == other;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode() => HICON.GetHashCode();

    public override bool Equals(
#if NET5_0_OR_GREATER
        [NotNullWhen(true)]
#endif
        object? obj)
        => obj?.GetHashCode() == GetHashCode();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Icon left, Icon right)
        => right.HICON.Value == left.HICON.Value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(Icon left, Icon right)
        => right.HICON.Value != left.HICON.Value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator IntPtr(Icon self) => self.HICON.Value;
}

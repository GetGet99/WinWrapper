using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.Graphics.Dwm;
using Windows.Win32.Graphics.Gdi;
using Windows.Win32.UI.WindowsAndMessaging;

namespace WinWrapper.Windowing;

partial struct Window
{
    public Rectangle Bounds
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            PInvoke.GetWindowRect(HWND, out var rect);
            return rect;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set
        {
            PInvoke.SetWindowPos(HWND, HWND.Null, value.X, value.Y, value.Width, value.Height,
                SET_WINDOW_POS_FLAGS.SWP_NOZORDER | SET_WINDOW_POS_FLAGS.SWP_NOACTIVATE | SET_WINDOW_POS_FLAGS.SWP_NOSENDCHANGING);
        }
    }
    public Rectangle ClientBounds
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            PInvoke.GetClientRect(HWND, out var rect);
            return rect;
        }
    }
    public Size Size
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Bounds.Size;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => Bounds = Bounds with { Size = value };
    }
    public Point Location
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Bounds.Location;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => PInvoke.SetWindowPos(HWND, HWND.Null, value.X, value.Y, 0, 0,
                SET_WINDOW_POS_FLAGS.SWP_NOZORDER | SET_WINDOW_POS_FLAGS.SWP_NOACTIVATE | SET_WINDOW_POS_FLAGS.SWP_NOSENDCHANGING | SET_WINDOW_POS_FLAGS.SWP_NOSIZE | SET_WINDOW_POS_FLAGS.SWP_NOCOPYBITS);
    }
    public Rectangle? Region
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            using var region = PInvoke.CreateRectRgn_SafeHandle(0, 0, 0, 0);
            if (PInvoke.GetWindowRgn(HWND, region) is GDI_REGION_TYPE.NULLREGION or GDI_REGION_TYPE.RGN_ERROR) return null;
            PInvoke.GetRgnBox(region, out var rect);
            return rect;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set
        {
            if (value.HasValue)
            {
                var rect = value.Value;
                using var region = PInvoke.CreateRectRgn_SafeHandle(rect.Left, rect.Top, rect.Right, rect.Bottom);
                PInvoke.SetWindowRgn(HWND, region, true);
            } else
            {
				PInvoke.SetWindowRgn(HWND, null, true);
			}
        }
    }
    public Task SetBoundsAsync(Rectangle NewBounds)
    {
        var self = this;
        return Task.Run(delegate
        {
            self.Bounds = NewBounds;
        });
    }
    public Task SetRegionAsync(Rectangle? NewRegion)
    {
        var self = this;
        return Task.Run(delegate
        {
            self.Region = NewRegion;
        });
    }
}

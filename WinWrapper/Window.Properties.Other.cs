using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.Graphics.Dwm;
using Windows.Win32.Graphics.Gdi;
using Windows.Win32.UI.WindowsAndMessaging;
using WinWrapper.Control;

namespace WinWrapper;

partial struct Window
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    int TitleTextLength => PInvoke.GetWindowTextLength(Handle);

    public string TitleText
    {
        get
        {
            unsafe
            {
                Span<char> chars = stackalloc char[TitleTextLength + 1];
                fixed (char* charptr = chars)
                {
                    PInvoke.GetWindowText(Handle, new PWSTR(charptr), TitleTextLength + 1);
                    
                }
                return new string(chars[..^1]);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => PInvoke.SetWindowText(Handle, value);
    }

    public WindowController Controller => new(this);

    public WINDOWPLACEMENT Placement
    {
        get
        {
            var placement = new WINDOWPLACEMENT();
            PInvoke.GetWindowPlacement(Handle, ref placement);
            return placement;
        }
    }
    public bool IsMaximized
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Placement.showCmd is SHOW_WINDOW_CMD.SW_MAXIMIZE;
    }
	public bool IsMinimized {
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => Placement.showCmd is SHOW_WINDOW_CMD.SW_MINIMIZE;
	}
	public bool IsNormalSize {
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => Placement.showCmd is SHOW_WINDOW_CMD.SW_NORMAL;
	}
	public bool IsVisible
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Style.HasFlag(WINDOW_STYLE.WS_VISIBLE);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => PInvoke.ShowWindow(Handle, value ? SHOW_WINDOW_CMD.SW_SHOW : SHOW_WINDOW_CMD.SW_HIDE);
    }
    public Rectangle Bounds
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            PInvoke.GetWindowRect(Handle, out var rect);
            return rect;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set
        {
            PInvoke.SetWindowPos(Handle, HWND.Null, value.X, value.Y, value.Width, value.Height,
                SET_WINDOW_POS_FLAGS.SWP_NOZORDER | SET_WINDOW_POS_FLAGS.SWP_NOACTIVATE | SET_WINDOW_POS_FLAGS.SWP_NOSENDCHANGING);
        }
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void ActivateSetWindowPos()
    {
        PInvoke.SetWindowPos(Handle, HWND.Null, 0, 0, 0, 0,
                SET_WINDOW_POS_FLAGS.SWP_NOSIZE | SET_WINDOW_POS_FLAGS.SWP_NOMOVE);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void ActivateTopMost()
    {
        PInvoke.SetWindowPos(Handle, new((nint)(-1)), 0, 0, 0, 0,
                SET_WINDOW_POS_FLAGS.SWP_NOSIZE | SET_WINDOW_POS_FLAGS.SWP_NOMOVE);
        PInvoke.SetWindowPos(Handle, new((nint)(-2)), 0, 0, 0, 0,
                SET_WINDOW_POS_FLAGS.SWP_NOSIZE | SET_WINDOW_POS_FLAGS.SWP_NOMOVE);
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
    public Rectangle ClientBounds
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            PInvoke.GetClientRect(Handle, out var rect);
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
        set => PInvoke.SetWindowPos(Handle, HWND.Null, value.X, value.Y, 0, 0,
                SET_WINDOW_POS_FLAGS.SWP_NOZORDER | SET_WINDOW_POS_FLAGS.SWP_NOACTIVATE | SET_WINDOW_POS_FLAGS.SWP_NOSENDCHANGING | SET_WINDOW_POS_FLAGS.SWP_NOSIZE | SET_WINDOW_POS_FLAGS.SWP_NOCOPYBITS);
    }
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public IntPtr SmallIconPtr
    {
        get
        {
            var handle = (nint)PInvoke.SendMessage(Handle, PInvoke.WM_GETICON, PInvoke.ICON_SMALL, 0);
            if (handle == 0) handle = (nint)PInvoke.SendMessage(Handle, PInvoke.WM_GETICON, PInvoke.ICON_SMALL2, 0);
            if (handle == 0) handle = PInvoke.GetClassLongPtr(Handle, GET_CLASS_LONG_INDEX.GCLP_HICONSM);

            return handle;
        }
        set
        {
            PInvoke.SendMessage(Handle, PInvoke.WM_SETICON, PInvoke.ICON_SMALL, value);
        }
    }
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public IntPtr LargeIconPtr
    {
        get
        {
            var handle = (nint)PInvoke.SendMessage(Handle, PInvoke.WM_GETICON, PInvoke.ICON_BIG, 0);
            if (handle == 0) handle = PInvoke.GetClassLongPtr(Handle, GET_CLASS_LONG_INDEX.GCLP_HICON);
            return handle;
        }
        set
        {
            PInvoke.SendMessage(Handle, PInvoke.WM_SETICON, PInvoke.ICON_BIG, value);
        }
    }
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Bitmap? SmallIcon
    {
        get
        {
            try
            {
                var handle = (nint)PInvoke.SendMessage(Handle, PInvoke.WM_GETICON, PInvoke.ICON_SMALL, 0);
                if (handle == 0) handle = (nint)PInvoke.SendMessage(Handle, PInvoke.WM_GETICON, PInvoke.ICON_SMALL2, 0);
                if (handle == 0) handle = PInvoke.GetClassLongPtr(Handle, GET_CLASS_LONG_INDEX.GCLP_HICONSM);
				if (handle == 0)
					return null;
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
                var handle = (nint)PInvoke.SendMessage(Handle, PInvoke.WM_GETICON, PInvoke.ICON_BIG, 0);
                if (handle == 0) handle = PInvoke.GetClassLongPtr(Handle, GET_CLASS_LONG_INDEX.GCLP_HICON);
				if (handle == 0)
					return null;
				return Bitmap.FromHicon(handle);
            }
            catch
            {
                return null;
            }

        }
    }
    public Display CurrentDisplay
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Display.FromWindow(this);
    }

    public WINDOWINFO Information
    {
        get
        {
            WINDOWINFO Info = new();
            PInvoke.GetWindowInfo(Handle, ref Info);
            return Info;
        }
    }
    /// <summary>
    /// Get The Class Name up to 256 characters
    /// </summary>
    public unsafe string ClassName
    {
        get
        {
            char* chars = stackalloc char[256];
            var str = new PWSTR(chars);
            _ = PInvoke.GetClassName(Handle, chars, 256);

            return new string(str);
        }
    }

    public Rectangle? Region
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            using var region = PInvoke.CreateRectRgn_SafeHandle(0, 0, 0, 0);
            if (PInvoke.GetWindowRgn(Handle, region) is GDI_REGION_TYPE.NULLREGION or GDI_REGION_TYPE.RGN_ERROR) return null;
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
                PInvoke.SetWindowRgn(Handle, region, true);
            } else
            {
				PInvoke.SetWindowRgn(Handle, null, true);
			}
        }
    }
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
    public unsafe T DwmGetWindowAttribute<T>(DWMWINDOWATTRIBUTE dwAttribute) where T : unmanaged
    {
        T ToReturn = new();
        PInvoke.DwmGetWindowAttribute(Handle, dwAttribute, &ToReturn, (uint)sizeof(T));
        return ToReturn;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public unsafe void DwmSetWindowAttribute<T>(DWMWINDOWATTRIBUTE dwAttribute, T value) where T : unmanaged
    {
        PInvoke.DwmSetWindowAttribute(Handle, dwAttribute, &value, (uint)sizeof(T));
    }
    public Process OwnerProcess
    {
        get => Process.FromHandle(PInvoke.GetProcessHandleFromHwnd(Handle));
    }
    public unsafe bool SetOverlayIconPtr(HICON Icon, string AlternateText)
    {
		try {
			fixed (char* alternateText = AlternateText) {
				Taskbars.ITaskbarList3.SetOverlayIcon(Handle, Icon, alternateText);
			}
		} catch (Exception) {
			return false;
		}//sometimes this api call may timeout and throw
		return true;
    }
    public unsafe void SetLayeredWindowBitmap(HBITMAP Bitmap, byte Opacity = 255)
    {
        var ScreenHDC = PInvoke.GetDC(default);
        var MemHDC = PInvoke.CreateCompatibleDC(ScreenHDC);
        HBITMAP OldBitmap = default;

        try
        {
            OldBitmap = new(PInvoke.SelectObject(new HDC(MemHDC.Value), new HGDIOBJ(Bitmap.Value)).Value);
            BLENDFUNCTION blend = new()
            {
                BlendOp = (byte)PInvoke.AC_SRC_OVER,
                BlendFlags = 0,
                SourceConstantAlpha = Opacity,
                AlphaFormat = (byte)PInvoke.AC_SRC_ALPHA
            };
            PInvoke.UpdateLayeredWindow(
                hWnd: Handle,
                hdcDst: ScreenHDC,
                pptDst: null,
                psize: null,
                hdcSrc: new HDC(MemHDC.Value),
                pptSrc: null,
                crKey: new COLORREF(0),
                pblend: &blend,
                dwFlags: UPDATE_LAYERED_WINDOW_FLAGS.ULW_ALPHA
            );
        } finally
        {
            PInvoke.ReleaseDC(default, ScreenHDC);
            if (!Bitmap.IsNull)
                PInvoke.SelectObject(new HDC(MemHDC.Value), new HGDIOBJ(OldBitmap.Value));
        }
    }
}

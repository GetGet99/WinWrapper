using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.Graphics.Gdi;
using Windows.Win32.UI.WindowsAndMessaging;
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
    }

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
    public bool IsVisible
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Placement.showCmd is not SHOW_WINDOW_CMD.SW_MINIMIZE or SHOW_WINDOW_CMD.SW_HIDE;

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
        set => Bounds = Bounds with { Location = value };
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
}
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
        get => Placement.showCmd is SHOW_WINDOW_CMD.SW_SHOWNORMAL;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => PInvoke.ShowWindow(Handle, value ? SHOW_WINDOW_CMD.SW_SHOW : SHOW_WINDOW_CMD.SW_HIDE);
    }
    public Rectangle Bounds
    {
        get
        {
            PInvoke.GetWindowRect(Handle, out var rect);
            return new Rectangle
            {
                X = rect.left,
                Y = rect.top,
                Width = rect.right - rect.left,
                Height = rect.bottom - rect.top
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set
        {
            PInvoke.SetWindowPos(Handle, HWND.Null, value.X, value.Y, value.Width, value.Height,
                SET_WINDOW_POS_FLAGS.SWP_NOZORDER | SET_WINDOW_POS_FLAGS.SWP_NOACTIVATE);
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
    public Icon? SmallIcon
    {
        get
        {
            try
            {
                var handle = (nint)PInvoke.DefWindowProc(Handle, PInvoke.WM_GETICON, 2, 0);
                if (handle == 0) handle = PInvoke.GetClassLongPtr(Handle, GET_CLASS_LONG_INDEX.GCLP_HICONSM);
                return Icon.FromHandle(handle);
            }
            catch
            {
                return null;
            }
        }
    }
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public Icon? LargeIcon
    {
        get
        {
            try
            {
                var handle = (nint)PInvoke.DefWindowProc(Handle, PInvoke.WM_GETICON, 1, 0);
                if (handle == 0) handle = (nint)PInvoke.GetClassLongPtr(Handle, GET_CLASS_LONG_INDEX.GCLP_HICON);
                return Icon.FromHandle(handle);
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
}
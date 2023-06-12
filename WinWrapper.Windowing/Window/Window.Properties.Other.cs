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
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    int TitleTextLength => PInvoke.GetWindowTextLength(HWND);

    public string TitleText
    {
        get
        {
            unsafe
            {
                Span<char> chars = stackalloc char[TitleTextLength + 1];
                fixed (char* charptr = chars)
                {
                    PInvoke.GetWindowText(HWND, new PWSTR(charptr), TitleTextLength + 1);
                }
#if NET5_0_OR_GREATER
                return new string(chars[..^1]);
#else
                return chars.Slice(chars.Length - 1).ToString();
#endif
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => PInvoke.SetWindowText(HWND, value);
    }

    internal WINDOWPLACEMENT Placement
    {
        get
        {
            var placement = new WINDOWPLACEMENT();
            PInvoke.GetWindowPlacement(HWND, ref placement);
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
        get => PInvoke.IsWindowVisible(HWND);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => PInvoke.ShowWindow(HWND, value ? SHOW_WINDOW_CMD.SW_SHOW : SHOW_WINDOW_CMD.SW_HIDE);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Activate(ActivationTechnique technique)
    {
        switch (technique)
        {
            case ActivationTechnique.SetWindowPos:
                PInvoke.SetWindowPos(HWND, HWND.Null, 0, 0, 0, 0,
                        SET_WINDOW_POS_FLAGS.SWP_NOSIZE | SET_WINDOW_POS_FLAGS.SWP_NOMOVE);
                break;
            case ActivationTechnique.SetWindowPosTopMost:
                PInvoke.SetWindowPos(HWND, new((nint)(-1)), 0, 0, 0, 0,
                SET_WINDOW_POS_FLAGS.SWP_NOSIZE | SET_WINDOW_POS_FLAGS.SWP_NOMOVE);
                PInvoke.SetWindowPos(HWND, new((nint)(-2)), 0, 0, 0, 0,
                        SET_WINDOW_POS_FLAGS.SWP_NOSIZE | SET_WINDOW_POS_FLAGS.SWP_NOMOVE);
                break;
        }
    }
    public void SetTopMost()
    {
        PInvoke.SetWindowPos(HWND, new((nint)(-1)), 0, 0, 0, 0,
                SET_WINDOW_POS_FLAGS.SWP_NOSIZE | SET_WINDOW_POS_FLAGS.SWP_NOMOVE);
    }
    
    public Display CurrentDisplay
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Display.FromHandle(PInvoke.MonitorFromWindow(HWND, MONITOR_FROM_FLAGS.MONITOR_DEFAULTTOPRIMARY));
    }

    internal WINDOWINFO Information
    {
        get
        {
            WINDOWINFO Info = new();
            PInvoke.GetWindowInfo(HWND, ref Info);
            return Info;
        }
    }
    /// <summary>
    /// Get The Class Name up to 256 characters
    /// </summary>
    public unsafe WindowClass Class
    {
        get
        {
            char* chars = stackalloc char[256];
            var str = new PWSTR(chars);
            _ = PInvoke.GetClassName(HWND, chars, 256);

            return WindowClass.FromExistingClass(new(str));
        }
    }

    public Process OwnerProcess
    {
        get => Process.FromHandle(PInvoke.GetProcessHandleFromHwnd(HWND));
    }

    public unsafe Thread Thread => Thread.FromHandle((nint)PInvoke.GetWindowThreadProcessId(HWND, (uint*)0));
}

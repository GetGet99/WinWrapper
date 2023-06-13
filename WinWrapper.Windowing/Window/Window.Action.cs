using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Windows.Win32;
using Windows.Win32.Graphics.Gdi;
using Windows.Win32.Foundation;
using Windows.Win32.UI.WindowsAndMessaging;
using System.Drawing.Imaging;
using System.Drawing;
using WinWrapper.Windowing.AdditionalPInvoke;
using WinWrapper.Input;
using System.ComponentModel;

namespace WinWrapper.Windowing;

partial struct Window
{
    /// <summary>
    /// Send the <see cref="PInvoke.WM_CLOSE"/> signel to the target <see cref="Window"/>
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void TryClose() => SendMessage(WindowMessages.Close, default(WPARAM), default(LPARAM));

    /// <summary>
    /// Send the <see cref="PInvoke.WM_CLOSE"/> signel to the target <see cref="Window"/>
    /// </summary>
    public async Task TryCloseAsync() => await Task.Run(TryClose);

    /// <summary>
    /// Focus the <see cref="Window"/>
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Focus()
        => PInvoke.SetFocus(HWND);

    /// <summary>
    /// Activate the <see cref="Window"/>
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Activate()
        => PInvoke.SetActiveWindow(HWND);

    /// <summary>
    /// Set the <see cref="Window"/> as Foreground Window
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void SetAsForegroundWindow()
        => PInvoke.SetForegroundWindow(HWND);

    /// <summary>
    /// Redraw the <see cref="Window"/>
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Redraw()
        => PInvoke.RedrawWindow(HWND, null, null, REDRAW_WINDOW_FLAGS.RDW_INVALIDATE);

    /// <summary>
    /// Shows the <see cref="Window"/>
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void SetVisibility(bool Visibility) => IsVisible = Visibility;

    /// <summary>
    /// Shows the <see cref="Window"/>
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public async Task SetVisibilityAsync(bool Visibility) => await Task.Run(Visibility ? Show : Hide);

    /// <summary>
    /// Shows the <see cref="Window"/>
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Show() => IsVisible = true;

    /// <summary>
    /// Hides the <see cref="Window"/>
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Hide() => IsVisible = false;

    /// <summary>
    /// Minimizes the <see cref="Window"/>
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Minimize() => PInvoke.ShowWindow(HWND, SHOW_WINDOW_CMD.SW_MINIMIZE);

    /// <summary>
    /// Minimizes the <see cref="Window"/>
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Restore() => PInvoke.ShowWindow(HWND, SHOW_WINDOW_CMD.SW_RESTORE);


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal LRESULT SendMessage(WindowMessages Message, WPARAM wPARAM, LPARAM lPARAM)
        => PInvoke.SendMessage(HWND, (uint)Message, wPARAM, lPARAM);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public nint SendMessage(WindowMessages Message, nuint wPARAM, nint lPARAM)
        => SendMessage(Message, new WPARAM(wPARAM), new LPARAM(lPARAM));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void SetLayeredWindowAttributes(Color Color, byte bAlpha, LayeredWindowAttributeFlags flags)
        => PInvoke.SetLayeredWindowAttributes(HWND,
            new((uint)ColorTranslator.ToWin32(Color)),
            bAlpha,
            (LAYERED_WINDOW_ATTRIBUTES_FLAGS)flags
        );

    public Bitmap PrintWindow()
    {
        // Refernce: https://stackoverflow.com/questions/891345/get-a-screenshot-of-a-specific-application
        var rc = Bounds;

        Bitmap bmp = new(rc.Width, rc.Height, PixelFormat.Format32bppArgb);
        Graphics gfxBmp = Graphics.FromImage(bmp);
        IntPtr hdcBitmap = gfxBmp.GetHdc();

        PInvoke.PrintWindow(HWND, new HDC(hdcBitmap), 0);

        gfxBmp.ReleaseHdc(hdcBitmap);
        gfxBmp.Dispose();

        return bmp;
    }

    public unsafe void SetAppId(string AppId)
    {
        Guid iid = new("886D8EEB-8CF2-4446-8D02-CDBA1DBDCF99");
        int result1 = SHGetPropertyStoreForWindow(HWND, ref iid, out IPropertyStore prop);
        // Name = System.AppUserModel.ID
        // ShellPKey = PKEY_AppUserModel_ID
        // FormatID = 9F4C2855-9F79-4B39-A8D0-E1D42DE1D5F3
        // PropID = 5
        // Type = String (VT_LPWSTR)
        PropertyKey AppUserModelIDKey = new("{9F4C2855-9F79-4B39-A8D0-E1D42DE1D5F3}", 5);

        PropVariant pv = new(AppId);

        uint result2 = prop.SetValue(ref AppUserModelIDKey, pv);

        Marshal.ReleaseComObject(prop);
    }

    /// <summary>
    /// Apply Drag Move. Call this only if user's primary mouse cursor is down
    /// </summary>
    public void DragMove()
    {
        Cursor.ReleaseCapture();
        SendMessage(WindowMessages.MouseLeftButtonUp, default(WPARAM), default);
        SendMessage(WindowMessages.SysCommand, PInvoke.SC_MOUSEMOVE, default(LPARAM));
    }

    /// <summary>
    /// Apply Drag Move. Call this only if user's primary mouse cursor is down
    /// </summary>
    public void DragMoveRightClick()
    {
        SendMessage(WindowMessages.SysCommand, PInvoke.SC_MOUSEMOVE, default(LPARAM));
        //SendMessage(WindowMessages.MouseRightButtonUp, default(WPARAM), default);
    }

    internal bool SetWindowPlacement(WINDOWPLACEMENT placement)
    {
        return PInvoke.SetWindowPlacement(HWND, in placement);
    }
    
    public void Maximize()
    {
        PInvoke.ShowWindow(HWND, SHOW_WINDOW_CMD.SW_MAXIMIZE);
    }

    public nint DefWindowProc(
        WindowMessages message,
        nuint wParam,
        nint lParam)
        => PInvoke.DefWindowProc(HWND, (uint)message, wParam, lParam);
    internal LRESULT DefWindowProc(
        WindowMessages message,
        WPARAM wParam,
        LPARAM lParam)
        => PInvoke.DefWindowProc(HWND, (uint)message, wParam, lParam);
    [DllImport("shell32.dll")]
    private static extern int SHGetPropertyStoreForWindow(
        HWND hwnd,
        ref Guid iid /*IID_IPropertyStore*/,
        [Out(), MarshalAs(UnmanagedType.Interface)] out IPropertyStore propertyStore);
    




    
}

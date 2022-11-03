using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Windows.Win32;
using Windows.Win32.Graphics.Gdi;
using Windows.Win32.Storage.Xps;
using Windows.Win32.Foundation;
using Windows.Win32.UI.WindowsAndMessaging;
using Windows.Win32.UI.Shell;
using System;
using System.Drawing.Imaging;
using System.Drawing;

namespace WinWrapper;

partial struct Window
{
    /// <summary>
    /// Send the <see cref="PInvoke.WM_CLOSE"/> signel to the target <see cref="Window"/>
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void TryClose() => PInvoke.SendMessage(Handle, PInvoke.WM_CLOSE, 0, 0);

    /// <summary>
    /// Send the <see cref="PInvoke.WM_CLOSE"/> signel to the target <see cref="Window"/>
    /// </summary>
    public async Task TryCloseAsync() => await Task.Run(TryClose);

    /// <summary>
    /// Focus the <see cref="Window"/>
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Focus()
        => PInvoke.SetFocus(Handle);

    /// <summary>
    /// Activate the <see cref="Window"/>
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Activate()
        => PInvoke.SetActiveWindow(Handle);

    /// <summary>
    /// Redraw the <see cref="Window"/>
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Redraw()
        => PInvoke.RedrawWindow(Handle, null, null, Windows.Win32.Graphics.Gdi.REDRAW_WINDOW_FLAGS.RDW_INVALIDATE);

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
    public void Minimize() => PInvoke.ShowWindow(Handle, SHOW_WINDOW_CMD.SW_MINIMIZE);

    /// <summary>
    /// Minimizes the <see cref="Window"/>
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Restore() => PInvoke.ShowWindow(Handle, SHOW_WINDOW_CMD.SW_RESTORE);

    public delegate LRESULT WndProcOverride(HWND Handle, uint code, WPARAM wParam, LPARAM lParam, WNDPROC Original);


    public Bitmap PrintWindow()
    {
        // Refernce: https://stackoverflow.com/questions/891345/get-a-screenshot-of-a-specific-application
        var rc = Bounds;

        Bitmap bmp = new(rc.Width, rc.Height, PixelFormat.Format32bppArgb);
        Graphics gfxBmp = Graphics.FromImage(bmp);
        IntPtr hdcBitmap = gfxBmp.GetHdc();

        PInvoke.PrintWindow(Handle, new HDC(hdcBitmap), 0);

        gfxBmp.ReleaseHdc(hdcBitmap);
        gfxBmp.Dispose();

        return bmp;
    }

    ///// <summary>
    ///// Minimizes the <see cref="Window"/>
    ///// </summary>
    //public void OverrideWndProc(WndProcOverride NewWndProc)
    //{
    //    WNDPROC? Original = default;
    //    try
    //    {
    //        GC.KeepAlive(NewWndProc);
    //        LRESULT newwndproc(HWND Handle, uint code, WPARAM wParam, LPARAM lParam) => NewWndProc.Invoke(Handle, code, wParam, lParam, Original!);
    //        WNDPROC newwndproce = newwndproc;
    //        GC.KeepAlive(newwndproce);
    //        var ptr = PInvoke.SetWindowLongPtr(Handle, WINDOW_LONG_PTR_INDEX.GWLP_WNDPROC, Marshal.GetFunctionPointerForDelegate(newwndproce));
    //        Original = Marshal.GetDelegateForFunctionPointer<WNDPROC>((nint)ptr);
    //        GC.KeepAlive(Original);
    //    } catch
    //    {
    //    }
    //}
    //public void SetSubclass(SUBCLASSPROC s, uint Id)
    //    => PInvoke.SetWindowSubclass(Handle, s, Id, UIntPtr.Zero);
}
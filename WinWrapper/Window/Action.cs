using System.Runtime.CompilerServices;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.WindowsAndMessaging;
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
    /// Shows the <see cref="Window"/>
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Show() => IsVisible = true;

    /// <summary>
    /// Hides the <see cref="Window"/>
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Hide() => IsVisible = true;

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
}
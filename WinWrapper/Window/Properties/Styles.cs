using System.Runtime.CompilerServices;
using Windows.Win32;
using Windows.Win32.UI.WindowsAndMessaging;
namespace WinWrapper;

partial struct Window
{
    /// <summary>
    /// Get or set the Style of the <see cref="Window"/>
    /// </summary>
    public WINDOW_STYLE Style
    {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => (WINDOW_STYLE)GetWindowLong(WINDOW_LONG_PTR_INDEX.GWL_STYLE);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => PInvoke.SetWindowLong(Handle, WINDOW_LONG_PTR_INDEX.GWL_STYLE, (int)value).ThrowOnFailure();
    }
    /// <summary>
    /// Get or set the Extended Style of the <see cref="Window"/>
    /// </summary>
    public WINDOW_EX_STYLE ExStyle
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => (WINDOW_EX_STYLE)GetWindowLong(WINDOW_LONG_PTR_INDEX.GWL_EXSTYLE);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => PInvoke.SetWindowLong(Handle, WINDOW_LONG_PTR_INDEX.GWL_EXSTYLE, (int)value).ThrowOnFailure();
    }
}
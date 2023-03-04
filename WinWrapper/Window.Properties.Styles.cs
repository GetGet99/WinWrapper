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
        set => _ = PInvoke.SetWindowLongPtr(Handle, WINDOW_LONG_PTR_INDEX.GWL_STYLE, (nint)value);
    }
	public void SetStyleFlag(WINDOW_STYLE style, bool enabled) {
		var curStyle = Style;
		var origStyle = curStyle;
		if (enabled)
			curStyle |= style;
		else
			curStyle &= ~style;
		if (curStyle != origStyle)
			Style = curStyle;
	}
	public void SetExStyleFlag(WINDOW_EX_STYLE style, bool enabled) {
		var curStyle = ExStyle;
		var origStyle = curStyle;
		if (enabled)
			curStyle |= style;
		else
			curStyle &= ~style;
		if (curStyle != origStyle)
			ExStyle = curStyle;
	}
	/// <summary>
	/// Get or set the Extended Style of the <see cref="Window"/>
	/// </summary>
	public WINDOW_EX_STYLE ExStyle
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => (WINDOW_EX_STYLE)GetWindowLong(WINDOW_LONG_PTR_INDEX.GWL_EXSTYLE);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => _ = PInvoke.SetWindowLongPtr(Handle, WINDOW_LONG_PTR_INDEX.GWL_EXSTYLE, (nint)value);
    }
}

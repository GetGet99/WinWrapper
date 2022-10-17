using System.Runtime.CompilerServices;
using Windows.Win32.UI.WindowsAndMessaging;
namespace WinWrapper;

partial struct Window
{
    /// <summary>
    /// Get or set the <see cref="WINDOW_STYLE.WS_SIZEBOX"/> style of the <see cref="Window"/> as <see cref="bool"/>
    /// </summary>
    public bool IsResizable
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => (Style & WINDOW_STYLE.WS_SIZEBOX) != 0;

        set
        {
            if (value)
                Style |= WINDOW_STYLE.WS_SIZEBOX;
            else
                Style &= ~WINDOW_STYLE.WS_SIZEBOX;
        }
    }
    //public bool IsVisibleStyle
    //{
    //    get => (Style & WINDOW_STYLE.WS_VISIBLE) != 0;
    //    set
    //    {
    //        if (value)
    //            Style |= WINDOW_STYLE.WS_VISIBLE;
    //        else
    //            Style &= ~WINDOW_STYLE.WS_VISIBLE;
    //    }
    //}

    /// <summary>
    /// Get or set the <see cref="WINDOW_STYLE.WS_CAPTION"/> style of the <see cref="Window"/> as <see cref="bool"/>
    /// </summary>
    public bool IsTtileBarVisible
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => (Style & WINDOW_STYLE.WS_CAPTION) != 0;

        set
        {
            if (value)
                Style |= WINDOW_STYLE.WS_CAPTION;
            else
                Style &= ~WINDOW_STYLE.WS_CAPTION;
        }
    }
}
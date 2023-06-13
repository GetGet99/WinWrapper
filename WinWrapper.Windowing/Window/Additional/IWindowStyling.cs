using System.Runtime.CompilerServices;
using Windows.Win32;

namespace WinWrapper.Windowing;

public interface IWindowStyleFlag
{
    bool this[WindowStyles style] { get; set; }
}
public interface IWindowExStyleFlag
{
    bool this[WindowExStyles style] { get; set; }
}
public interface IWindowStyling : IWindowStyleFlag, IWindowExStyleFlag
{
    /// <summary>
    /// Get or set the Style of the <see cref="Window"/>
    /// </summary>
    WindowStyles Style { get; set; }
    /// <summary>
    /// Get or set the Extended Style of the <see cref="Window"/>
    /// </summary>
    WindowExStyles ExStyle { get; set; }
    IWindowStyleFlag StyleFlag { get; }
    IWindowExStyleFlag ExStyleFlag { get; }

    bool IsResizable { get; set; }
    bool IsTtileBarVisible { get; set; }
}

partial struct Window : IWindowStyling, IWindowStyleFlag, IWindowExStyleFlag
{
    public IWindowStyling Styling
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => this;
    }

    IWindowStyleFlag IWindowStyling.StyleFlag
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => this;
    }
    IWindowExStyleFlag IWindowStyling.ExStyleFlag
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => this;
    }


    public WindowStyles Style
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => (WindowStyles)WindowLong[WindowLongPtrIndex.GWL_Style];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => WindowLong[WindowLongPtrIndex.GWL_Style] = (nint)value;
    }
    public WindowExStyles ExStyle
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => (WindowExStyles)WindowLong[WindowLongPtrIndex.GWL_ExStyle];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => WindowLong[WindowLongPtrIndex.GWL_ExStyle] = (nint)value;
    }

    /// <summary>
    /// Get or set the <see cref="WindowStyles.SizeBox"/> style of the <see cref="Window"/> as <see cref="bool"/>
    /// </summary>
    public bool IsResizable
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Styling[WindowStyles.SizeBox];

        set => Styling[WindowStyles.SizeBox] = value;

    }

    /// <summary>
    /// Get or set the <see cref="WindowStyles.Caption"/> style of the <see cref="Window"/> as <see cref="bool"/>
    /// </summary>
    public bool IsTtileBarVisible
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Styling[WindowStyles.Caption];

        set => Styling[WindowStyles.Caption] = value;
    }

    public bool this[WindowExStyles style] {
        get => (Styling.ExStyle & style) != 0;
        set
        {
            var curStyle = Styling.ExStyle;
            var origStyle = curStyle;
            if (value)
                curStyle |= style;
            else
                curStyle &= ~style;
            if (curStyle != origStyle)
                Styling.ExStyle = curStyle;
        }
    }
    public bool this[WindowStyles style]
    {
        get => (Styling.Style & style) != 0;
        set
        {
            var curStyle = Styling.Style;
            var origStyle = curStyle;
            if (value)
                curStyle |= style;
            else
                curStyle &= ~style;
            if (curStyle != origStyle)
                Styling.Style = curStyle;
        }
    }
}
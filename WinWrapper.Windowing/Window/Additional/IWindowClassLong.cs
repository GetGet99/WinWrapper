using System.Runtime.CompilerServices;
using Windows.Win32;

namespace WinWrapper.Windowing;

public interface IWindowClassLong
{
    nuint this[WindowClassLongIndex flag] { get; set; }
    WindowClassStyles Style { get; set; }
}
public partial struct Window : IWindowClassLong
{
    public IWindowClassLong WindowClassLong
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => this;
    }

    public readonly nuint this[WindowClassLongIndex flag]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => PInvoke.GetClassLongAuto(HWND, flag);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => PInvoke.SetClassLongAuto(HWND, flag, value);
    }
    WindowClassStyles IWindowClassLong.Style {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => (WindowClassStyles) (this as IWindowClassLong)[WindowClassLongIndex.GCL_STYLE];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => (this as IWindowClassLong)[WindowClassLongIndex.GCL_STYLE] = (nuint)value;
    }

    public WindowClassStyles ClassStyle
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => WindowClassLong.Style;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => WindowClassLong.Style = value;
    }
}


using System.Runtime.CompilerServices;
using Windows.Win32;

namespace WinWrapper.Windowing;

public interface IWindowLong
{
    nint this[WindowLongPtrIndex flag] { get; set; }
}
public partial struct Window : IWindowLong
{
    public IWindowLong WindowLong
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => this;
    }
    public readonly nint this[WindowLongPtrIndex flag]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => PInvoke.GetWindowLongAuto(HWND, flag);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => PInvoke.SetWindowLongAuto(HWND, flag, value);
    }
}


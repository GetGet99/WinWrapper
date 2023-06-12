using System.Runtime.CompilerServices;
using Windows.Win32;
using Windows.Win32.Graphics.Dwm;

namespace WinWrapper.Windowing;
public interface IDwmWindowAttribute
{
    T Get<T>(DwmWindowAttribute dwAttribute) where T : unmanaged;
    void Set<T>(DwmWindowAttribute dwAttribute, T value) where T : unmanaged;
}
public partial struct Window : IDwmWindowAttribute
{
    IDwmWindowAttribute DwmAttribute => this;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    readonly unsafe T IDwmWindowAttribute.Get<T>(DwmWindowAttribute dwAttribute)
    {
        T ToReturn = new();
        PInvoke.DwmGetWindowAttribute(HWND, (DWMWINDOWATTRIBUTE)dwAttribute, &ToReturn, (uint)sizeof(T));
        return ToReturn;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    readonly unsafe void IDwmWindowAttribute.Set<T>(DwmWindowAttribute dwAttribute, T value)
    {
        PInvoke.DwmSetWindowAttribute(HWND, (DWMWINDOWATTRIBUTE)dwAttribute, &value, (uint)sizeof(T));
    }
}


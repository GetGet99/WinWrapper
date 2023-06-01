using System.Runtime.CompilerServices;
using Windows.Win32;
using Windows.Win32.UI.WindowsAndMessaging;
namespace WinWrapper;

partial struct Window
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int GetWindowLong(WINDOW_LONG_PTR_INDEX flag) => PInvoke.GetWindowLong(Handle, flag);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int SetWindowLong(WINDOW_LONG_PTR_INDEX index, int setflag) => PInvoke.SetWindowLong(Handle, index, setflag);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int SetWindowLong(WINDOW_LONG_PTR_INDEX index, IntPtr setflag)
        => PInvoke.SetWindowLong(Handle, index, (int)setflag);

    public WNDCLASS_STYLES ClassStyle
    {
        get => (WNDCLASS_STYLES)PInvoke.GetClassLong(Handle, GET_CLASS_LONG_INDEX.GCL_STYLE);
        set => PInvoke.SetClassLong(Handle, GET_CLASS_LONG_INDEX.GCL_STYLE, (int)value);
    }
}
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Windows.Win32;
using Windows.Win32.UI.WindowsAndMessaging;
namespace WinWrapper.Windowing;

partial struct Window
{
    /// <summary>
    /// Retrives all the children <see cref="Window"/>s
    /// </summary>
    public List<Window> Children
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => GetWindowAPI.EnumChildWindows(HWND);
    }

    /// <summary>
    /// Get or set the owner <see cref="Window"/>
    /// </summary>
    public Window Owner
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(PInvoke.GetWindow(HWND, GET_WINDOW_CMD.GW_OWNER));
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => WindowLong[WindowLongPtrIndex.GWL_HWNDPARENT] = value.HWND;
    }

    /// <summary>
    /// Get or set the parent <see cref="Window"/>
    /// </summary>
    public Window Parent
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(PInvoke.GetAncestor(HWND, GET_ANCESTOR_FLAGS.GA_PARENT));
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => PInvoke.SetParent(HWND, value.HWND);
    }

    /// <summary>
    /// Retrives the root <see cref="Window"/>
    /// </summary>
    public Window Root
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(PInvoke.GetAncestor(HWND, GET_ANCESTOR_FLAGS.GA_ROOT));
    }
}
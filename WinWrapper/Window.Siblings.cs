using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Windows.Win32;
using Windows.Win32.UI.WindowsAndMessaging;
namespace WinWrapper;

partial struct Window
{
    /// <summary>
    /// Retrives all the children <see cref="Window"/>s
    /// </summary>
    public Span<Window> Children
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => CollectionsMarshal.AsSpan(GetWindowAPI.EnumChildWindows(Handle));
    }

    /// <summary>
    /// Get or set the owner <see cref="Window"/>
    /// </summary>
    public Window Owner
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(PInvoke.GetWindow(Handle, GET_WINDOW_CMD.GW_OWNER));
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set
        {
            _ = PInvoke.SetWindowLongPtr(Handle, WINDOW_LONG_PTR_INDEX.GWL_HWNDPARENT, value.Handle.Value);
        }
    }

    /// <summary>
    /// Get or set the parent <see cref="Window"/>
    /// </summary>
    public Window Parent
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(PInvoke.GetAncestor(Handle, GET_ANCESTOR_FLAGS.GA_PARENT));
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => PInvoke.SetParent(Handle, value.Handle);
    }

    /// <summary>
    /// Retrives the root <see cref="Window"/>
    /// </summary>
    public Window Root
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(PInvoke.GetAncestor(Handle, GET_ANCESTOR_FLAGS.GA_ROOT));
    }
}
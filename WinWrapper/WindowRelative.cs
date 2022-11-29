using System.Runtime.CompilerServices;
using Windows.Win32;
using Windows.Win32.UI.WindowsAndMessaging;

namespace WinWrapper;

public readonly struct WindowRelative
{
    /// <summary>
    /// Get the <see cref="Window"/> reference
    /// </summary>
    public Window LinkedWindow { get; }
    public WindowRelative(Window Window)
    {
        this.LinkedWindow = Window;
    }
    /// <summary>
    /// Get the <see cref="Window"/> above the current one
    /// </summary>
    public Window Above
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Window.FromWindowHandle(PInvoke.GetWindow(LinkedWindow.Handle, GET_WINDOW_CMD.GW_HWNDPREV));
    }
    /// <summary>
    /// Get the <see cref="Window"/> below the current one
    /// </summary>
    public Window Below
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Window.FromWindowHandle(PInvoke.GetWindow(LinkedWindow.Handle, GET_WINDOW_CMD.GW_HWNDNEXT));
    } 
    static IEnumerable<Window> GetBelows(Window refernece)
    {
        while (true)
        {
            refernece = new WindowRelative(refernece).Below;
            if (refernece.IsValid)
                yield return refernece;
            else
                yield break;
        }
    }
    /// <summary>
    /// Get all the <see cref="Window"/>s below the current one
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IEnumerable<Window> GetBelows() => GetBelows(LinkedWindow);
    static IEnumerable<Window> GetAboves(Window refernece)
    {
        while (true)
        {
            refernece = new WindowRelative(refernece).Above;
            if (refernece.IsValid)
                yield return refernece;
            else
                yield break;
        }
    }
    /// <summary>
    /// Get all the <see cref="Window"/>s above the current one
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IEnumerable<Window> GetAboves() => GetAboves(LinkedWindow);
}

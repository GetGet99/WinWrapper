using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Windows.Win32;
using Windows.Win32.Foundation;
namespace WinWrapper.Windowing;

public readonly partial struct Window
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private Window(HWND Handle)
    {
        HWND = Handle;
    }
    /// <summary>
    /// Retrives the <see cref="Windows.Win32.Foundation.HWND"/> of the <see cref="Window"/>
    /// </summary>
    internal readonly HWND HWND;
    /// <summary>
    /// Retrives the <see cref="nint"/> of the <see cref="Window"/>
    /// </summary>
    public readonly nint Handle => HWND.Value;
    /// <summary>
    /// Check if <see cref="Window"/> is a valid window
    /// </summary>
    public bool IsValid
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => PInvoke.IsWindow(HWND);
    }
}
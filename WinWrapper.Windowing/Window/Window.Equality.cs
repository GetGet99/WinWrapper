using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Windows.Win32.Foundation;
namespace WinWrapper.Windowing;

public readonly partial struct Window
{
    /// <summary>
    /// Returns the readable string representation of <see cref="Window"/>
    /// </summary>
    public override string ToString()
    {
        return IsValid ? $"Window {HWND.Value} {TitleText} ({Class.ClassName})" : $"Invalid Window ({HWND.Value})";
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(Window other)
        => this == other;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode() => HWND.GetHashCode();

    public override bool Equals(
#if NET5_0_OR_GREATER
        [NotNullWhen(true)]
#endif
        object? obj)
        => obj?.GetHashCode() == GetHashCode();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Window left, Window right)
        => right.HWND.Value == left.HWND.Value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(Window left, Window right)
        => right.HWND.Value != left.HWND.Value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator IntPtr(Window win) => win.HWND.Value;
}
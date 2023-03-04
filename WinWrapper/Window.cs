using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;
using Windows.Win32;
using Windows.Win32.Foundation;
namespace WinWrapper;

public readonly partial struct Window
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private Window(HWND Handle)
    {
        this.Handle = new HWND(Handle);
    }
    /// <summary>
    /// Retrives the <see cref="HWND"/> of the <see cref="Window"/>
    /// </summary>
    public readonly HWND Handle;
    /// <summary>
    /// Check if <see cref="Window"/> is a valid window
    /// </summary>
    public bool IsValid
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => PInvoke.IsWindow(Handle);
    }
    /// <summary>
    /// Returns the readable string representation of <see cref="Window"/>
    /// </summary>
    public override string ToString()
    {
        return IsValid ? $"Window {Handle.Value} {TitleText} ({ClassName})" : $"Invalid Window ({Handle.Value})";
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(Window other)
        => this == other;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode() => Handle.GetHashCode();

    public override bool Equals([NotNullWhen(true)] object? obj)
        => obj?.GetHashCode() == GetHashCode();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Window left, Window right)
        => right.Handle.Value == left.Handle.Value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(Window left, Window right)
        => right.Handle.Value != left.Handle.Value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator IntPtr(Window win) => win.Handle.Value;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator HWND(Window win) => win.Handle;
}
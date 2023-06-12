using System.Drawing;
using System.Runtime.CompilerServices;
using Windows.Win32;
namespace WinWrapper.Input;

public static class Cursor
{
    public static bool IsLeftButtonDown
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Keyboard.IsKeyDown(VirtualKey.MouseLeftButton);
    }
    public static bool IsRightButtonDown
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Keyboard.IsKeyDown(VirtualKey.MouseRightButton);
    }
    public static bool IsX1ButtonDown
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Keyboard.IsKeyDown(VirtualKey.MouseXButton1);
    }
    public static bool IsX2ButtonDown
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Keyboard.IsKeyDown(VirtualKey.MouseXButton2);
    }
    public static Point Position
    {
        get
        {
            if (PInvoke.GetCursorPos(out var pt))
                return pt;
            else return default;
        }
    }
    public static bool ReleaseCapture()
        => PInvoke.ReleaseCapture();
}

public static class Keyboard
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsKeyDown(VirtualKey Key) => PInvoke.GetAsyncKeyState((int)Key) != 0;

    public static bool IsShiftDown
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => IsKeyDown(VirtualKey.Shift);
    }
    public static bool IsAltDown
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => IsKeyDown(VirtualKey.Menu);
    }

    public static bool IsControlDown
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => IsKeyDown(VirtualKey.Control);
    }
}
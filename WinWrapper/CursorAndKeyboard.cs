using System.Drawing;
using System.Runtime.CompilerServices;
using Windows.Win32;
using Windows.Win32.UI.Input.KeyboardAndMouse;
namespace WinWrapper;

public static class Cursor
{
    public static bool IsLeftButtonDown
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Keyboard.GetAsyncKeyDown(VIRTUAL_KEY.VK_LBUTTON);
    }
    public static bool IsRightButtonDown
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Keyboard.GetAsyncKeyDown(VIRTUAL_KEY.VK_RBUTTON);
    }
    public static bool IsX1ButtonDown
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Keyboard.GetAsyncKeyDown(VIRTUAL_KEY.VK_XBUTTON1);
    }
    public static bool IsX2ButtonDown
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Keyboard.GetAsyncKeyDown(VIRTUAL_KEY.VK_XBUTTON2);
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
}

public static class Keyboard
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool GetAsyncKeyDown(VIRTUAL_KEY Key) => PInvoke.GetAsyncKeyState((int)Key) != 0;

    public static bool IsShiftDown
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => GetAsyncKeyDown(VIRTUAL_KEY.VK_SHIFT);
    }

    public static bool IsControlDown
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => GetAsyncKeyDown(VIRTUAL_KEY.VK_CONTROL);
    }
}

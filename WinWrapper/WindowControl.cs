using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.Win32;
using Windows.Win32.UI.Input.KeyboardAndMouse;

namespace WinWrapper.Control;
public enum ActivateStatus : byte
{
    Active = 1,
    ClickActive = 2,
    Inactive = 0
}
public enum MouseEventKind : uint
{
    LeftButtonDown = 0x0201,
    LeftButtonUp = 0x0202,
    LeftButtonDoubleClick = 0x0203,
    RightButtonDown = 0x0204,
    RightButtonUp = 0x0205,
    RightButtonDoubleClick = 0x0206,
    Move = 0x0200
}
public readonly struct WindowController
{
    public readonly Window Window;
    public WindowController(Window window)
    {
        Window = window;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void FakeActivate(ActivateStatus activateStatus = ActivateStatus.ClickActive)
    {
        Window.SendMessage(
            PInvoke.WM_ACTIVATE,
            new((nuint)activateStatus),
            new(Window.Handle)
        );
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void SendMouse(MouseEventKind EventKind, Point Location)
    {
        Window.SendMessage(
            (uint)EventKind,
            0,
            Location.Y * 0x10000 + Location.X
        );
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void SendKey(bool IsKeyDown, VIRTUAL_KEY KeyCode)
    {
        Window.SendMessage(
            IsKeyDown ? PInvoke.WM_KEYDOWN : PInvoke.WM_KEYUP,
            new((nuint)KeyCode),
            IsKeyDown ? 0 : 1
        );
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void SendChar(char Char)
    {
        Window.SendMessage(
            PInvoke.WM_CHAR,
            Char,
            0
        );
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void SendString(string s)
    {
        foreach (char c in s) SendChar(c);
    }
}

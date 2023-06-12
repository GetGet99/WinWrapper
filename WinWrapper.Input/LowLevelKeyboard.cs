// The original file was from United Sets project, which is moved and modified to the code here
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.WindowsAndMessaging;
using WinWrapper.Input;

namespace WinWrapper;

public delegate void LowLevelKeyboardEventHandle(KeyboardHookInfo eventDetails, KeyboardState state, ref bool Handled);
public class LowLevelKeyboard : IDisposable
{
    static LowLevelKeyboard? Singleton;
    static LowLevelKeyboardEventHandle? _KeyboardPressed;
    public static event LowLevelKeyboardEventHandle? KeyPressed
    {
        add
        {
            _KeyboardPressed += value;
            Singleton ??= new();
        }
        remove
        {
            _KeyboardPressed -= value;
            if (_KeyboardPressed is null) DestroySingleton();
        }
    }
    static void DestroySingleton()
    {
        Singleton?.Dispose();
        Singleton = null;
    }
    public static void UnregisterAll() {
        _KeyboardPressed = null;
        DestroySingleton();
    }

    private readonly FreeLibrarySafeHandle _user32LibraryHandle;
    private HOOKPROC? _hookProc;
    private readonly UnhookWindowsHookExSafeHandle _windowsHookHandle;

    private LowLevelKeyboard()
    {
        _hookProc = LowLevelKeyboardProc;
        _user32LibraryHandle = PInvoke.LoadLibrary("User32");

        if (_user32LibraryHandle.IsInvalid)
        {
            int errorCode = Marshal.GetLastWin32Error();
            throw new Win32Exception(
                errorCode,
                $"Failed to load library 'User32.dll'. Error {errorCode}: {
                    new Win32Exception(errorCode).Message
                }."
            );
        }

        _windowsHookHandle = PInvoke.SetWindowsHookEx(
            WINDOWS_HOOK_ID.WH_KEYBOARD_LL,
            _hookProc,
            _user32LibraryHandle,
            0
        );

        if (_windowsHookHandle.IsInvalid)
        {
            int errorCode = Marshal.GetLastWin32Error();
            throw new Win32Exception(errorCode, $"Failed to adjust keyboard hooks for '{
                System.Diagnostics.Process.GetCurrentProcess().ProcessName
                }'. Error {errorCode}: {new Win32Exception(errorCode).Message}."
            );
        }
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            // because we can unhook only in the same thread, not in garbage collector thread
            if (!_windowsHookHandle.IsInvalid)
            {
                _windowsHookHandle.Dispose();
                _hookProc -= LowLevelKeyboardProc;
            }
        }

        if (_user32LibraryHandle.IsInvalid) return;
        {
            _user32LibraryHandle.Dispose();
        }
    }

    ~LowLevelKeyboard()
    {
        Dispose(false);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private unsafe LRESULT LowLevelKeyboardProc(int nCode, WPARAM wParam, LPARAM lParam)
    {
        bool fEatKeyStroke = false;
        
        if (nCode < 0) goto End;

        var obj = *(KBDLLHOOKSTRUCT*)lParam.Value;

        _KeyboardPressed?.Invoke(
            new()
            {
                DWExtraInfo = obj.dwExtraInfo,
                Flags = (KeyboardHookInfoFlags)obj.flags,
                ScanCode = obj.scanCode,
                Time = obj.time,
                KeyCode = (VirtualKey)obj.vkCode
            },
            (KeyboardState)wParam.Value,
            ref fEatKeyStroke
        );
    End:

        return fEatKeyStroke ?
            new(1) :
            PInvoke.CallNextHookEx(null, nCode, wParam, lParam);
    }
}
public enum KeyboardState
{
    KeyDown = 0x0100,
    KeyUp = 0x0101,
    SystemKeyDown = 0x0104,
    SystemKeyUp = 0x0105
}

[global::System.CodeDom.Compiler.GeneratedCode("Microsoft.Windows.CsWin32", "0.3.2-beta+d18600d19b")]
public partial struct KeyboardHookInfo
{
    /// <summary>
    /// <para>Type: <b>DWORD</b> A <a href="https://docs.microsoft.com/windows/desktop/inputdev/virtual-key-codes">virtual-key code</a>. The code must be a value in the range 1 to 254.</para>
    /// <para><see href="https://docs.microsoft.com/windows/win32/api/winuser/ns-winuser-kbdllhookstruct#members">Read more on docs.microsoft.com</see>.</para>
    /// </summary>
    public VirtualKey KeyCode;

    /// <summary>
    /// <para>Type: <b>DWORD</b> A hardware scan code for the key.</para>
    /// <para><see href="https://docs.microsoft.com/windows/win32/api/winuser/ns-winuser-kbdllhookstruct#members">Read more on docs.microsoft.com</see>.</para>
    /// </summary>
    public uint ScanCode;

    /// <summary>
    /// <para>Type: <b>DWORD</b> The extended-key flag, event-injected flags, context code, and transition-state flag. This member is specified as follows. An application can use the following values to test the keystroke flags. Testing LLKHF_INJECTED (bit 4) will tell you whether the event was injected. If it was, then testing LLKHF_LOWER_IL_INJECTED (bit 1) will tell you whether or not the event was injected from a process running at lower integrity level. </para>
    /// <para>This doc was truncated.</para>
    /// <para><see href="https://docs.microsoft.com/windows/win32/api/winuser/ns-winuser-kbdllhookstruct#members">Read more on docs.microsoft.com</see>.</para>
    /// </summary>
    public KeyboardHookInfoFlags Flags;

    /// <summary>
    /// <para>Type: <b>DWORD</b> The time stamp for this message, equivalent to what <a href="https://docs.microsoft.com/windows/desktop/api/winuser/nf-winuser-getmessagetime">GetMessageTime</a> would return for this message.</para>
    /// <para><see href="https://docs.microsoft.com/windows/win32/api/winuser/ns-winuser-kbdllhookstruct#members">Read more on docs.microsoft.com</see>.</para>
    /// </summary>
    public uint Time;

    /// <summary>
    /// <para>Type: <b>ULONG_PTR</b> Additional information associated with the message.</para>
    /// <para><see href="https://docs.microsoft.com/windows/win32/api/winuser/ns-winuser-kbdllhookstruct#members">Read more on docs.microsoft.com</see>.</para>
    /// </summary>
    public nuint DWExtraInfo;
}
public enum KeyboardHookInfoFlags : uint
{
    LLKHF_EXTENDED = KBDLLHOOKSTRUCT_FLAGS.LLKHF_EXTENDED,
    LLKHF_ALTDOWN = KBDLLHOOKSTRUCT_FLAGS.LLKHF_ALTDOWN,
    LLKHF_UP = KBDLLHOOKSTRUCT_FLAGS.LLKHF_UP,
    LLKHF_INJECTED = KBDLLHOOKSTRUCT_FLAGS.LLKHF_INJECTED,
    LLKHF_LOWER_IL_INJECTED = KBDLLHOOKSTRUCT_FLAGS.LLKHF_LOWER_IL_INJECTED,
}
// The original file was from United Sets project, which is moved and modified to the code here
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Windows.System;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.WindowsAndMessaging;

namespace WinWrapper;

public delegate void LowLevelKeyboardEventHandle(KBDLLHOOKSTRUCT eventDetails, KeyboardState state, ref bool Handled);
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

        
        _KeyboardPressed?.Invoke(
            *(KBDLLHOOKSTRUCT*)lParam.Value,
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
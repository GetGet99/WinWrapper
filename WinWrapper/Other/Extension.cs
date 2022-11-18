using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Windows.Win32.Foundation;

namespace WinWrapper;

static class Extension
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowOnFailure(this int ErrorCode)
    {
        if (ErrorCode != 0) throw new COMException("An Error has occured", ErrorCode);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowOnFailure(this BOOL Boolean)
    {
        if (Boolean) return;
        if (Debugger.IsAttached) Debugger.Break();
        throw new COMException($"An Error has occured: {Marshal.GetLastWin32Error()}");
    }
}

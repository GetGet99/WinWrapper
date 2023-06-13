using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using Windows.Win32.Foundation;
using Windows.Win32.Graphics.Gdi;
using Windows.Win32.UI.Shell;
using Windows.Win32.UI.WindowsAndMessaging;
using WinWrapper;
using WinWrapper.Windowing;

namespace Windows.Win32;
#pragma warning disable CA1401
partial class PInvoke
{
    /// <summary>Changes an attribute of the specified window. The function also sets the 32-bit (long) value at the specified offset into the extra window memory.</summary>
    /// <param name="hWnd">
    /// <para>Type: <b>HWND</b> A handle to the window and, indirectly, the class to which the window belongs.</para>
    /// <para><see href="https://docs.microsoft.com/windows/win32/api//winuser/nf-winuser-setwindowlongptrw#parameters">Read more on docs.microsoft.com</see>.</para>
    /// </param>
    /// <param name="nIndex">Type: <b>int</b></param>
    /// <param name="dwNewLong">
    /// <para>Type: <b>LONG</b> The replacement value.</para>
    /// <para><see href="https://docs.microsoft.com/windows/win32/api//winuser/nf-winuser-setwindowlongptrw#parameters">Read more on docs.microsoft.com</see>.</para>
    /// </param>
    /// <returns>
    /// <para>Type: <b>LONG</b> If the function succeeds, the return value is the previous value of the specified 32-bit integer. If the function fails, the return value is zero. To get extended error information, call <a href="/windows/desktop/api/errhandlingapi/nf-errhandlingapi-getlasterror">GetLastError</a>. If the previous value of the specified 32-bit integer is zero, and the function succeeds, the return value is zero, but the function does not clear the last error information. This makes it difficult to determine success or failure. To deal with this, you should clear the last error information by calling <a href="/windows/desktop/api/errhandlingapi/nf-errhandlingapi-setlasterror">SetLastError</a> with 0 before calling <b>SetWindowLong</b>. Then, function failure will be indicated by a return value of zero and a <a href="/windows/desktop/api/errhandlingapi/nf-errhandlingapi-getlasterror">GetLastError</a> result that is nonzero.</para>
    /// </returns>
    /// <remarks>
    /// <para><see href="https://docs.microsoft.com/windows/win32/api//winuser/nf-winuser-setwindowlongptrw">Learn more about this API from docs.microsoft.com</see>.</para>
    /// </remarks>
    [DllImport("USER32.dll", ExactSpelling = true, EntryPoint = "SetWindowLongPtrW", SetLastError = true)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
#if NET5_0_OR_GREATER
    [SupportedOSPlatform("windows5.0")]
#endif
    private static extern IntPtr SetWindowLongPtr(HWND hWnd, WINDOW_LONG_PTR_INDEX nIndex, IntPtr dwNewLong);

    internal static nint SetWindowLongAuto(HWND hwnd, WindowLongPtrIndex nIndex, nint dwNewLong)
        => AssemblyInfo.Is64Bit ?
            SetWindowLongPtr(hwnd, (WINDOW_LONG_PTR_INDEX)nIndex, dwNewLong) :
            SetWindowLong(hwnd, (WINDOW_LONG_PTR_INDEX)nIndex, (int)dwNewLong);

    /// <summary>Retrieves information about the specified window. The function also retrieves the value at a specified offset into the extra window memory. (Unicode)</summary>
    /// <param name="hWnd">
    /// <para>Type: <b>HWND</b> A handle to the window and, indirectly, the class to which the window belongs.</para>
    /// <para><see href="https://docs.microsoft.com/windows/win32/api/winuser/nf-winuser-getwindowlongptrw#parameters">Read more on docs.microsoft.com</see>.</para>
    /// </param>
    /// <param name="nIndex">Type: <b>int</b></param>
    /// <returns>
    /// <para>Type: <b>LONG_PTR</b> If the function succeeds, the return value is the requested value. If the function fails, the return value is zero. To get extended error information, call <a href="https://docs.microsoft.com/windows/desktop/api/errhandlingapi/nf-errhandlingapi-getlasterror">GetLastError</a>. If <a href="https://docs.microsoft.com/windows/desktop/api/winuser/nf-winuser-setwindowlonga">SetWindowLong</a> or <a href="https://docs.microsoft.com/windows/desktop/api/winuser/nf-winuser-setwindowlongptra">SetWindowLongPtr</a> has not been called previously, <b>GetWindowLongPtr</b> returns zero for values in the extra window or class memory.</para>
    /// </returns>
    /// <remarks>
    /// <para>Reserve extra window memory by specifying a nonzero value in the <b>cbWndExtra</b> member of the <a href="https://docs.microsoft.com/windows/desktop/api/winuser/ns-winuser-wndclassexa">WNDCLASSEX</a> structure used with the <a href="https://docs.microsoft.com/windows/desktop/api/winuser/nf-winuser-registerclassexa">RegisterClassEx</a> function.</para>
    /// <para>> [!NOTE] > The winuser.h header defines GetWindowLongPtr as an alias which automatically selects the ANSI or Unicode version of this function based on the definition of the UNICODE preprocessor constant. Mixing usage of the encoding-neutral alias with code that not encoding-neutral can lead to mismatches that result in compilation or runtime errors. For more information, see [Conventions for Function Prototypes](/windows/win32/intl/conventions-for-function-prototypes).</para>
    /// <para><see href="https://docs.microsoft.com/windows/win32/api/winuser/nf-winuser-getwindowlongptrw#">Read more on docs.microsoft.com</see>.</para>
    /// </remarks>
    [DllImport("USER32.dll", ExactSpelling = true, EntryPoint = "GetWindowLongPtrW", SetLastError = true)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
#if NET5_0_OR_GREATER
    [SupportedOSPlatform("windows5.0")]
#endif
    private static extern nint GetWindowLongPtr(HWND hWnd, WINDOW_LONG_PTR_INDEX nIndex);

    internal static nint GetWindowLongAuto(HWND hwnd, WindowLongPtrIndex nIndex)
        => AssemblyInfo.Is64Bit ?
            GetWindowLongPtr(hwnd, (WINDOW_LONG_PTR_INDEX)nIndex) :
            GetWindowLong(hwnd, (WINDOW_LONG_PTR_INDEX)nIndex);

    /// <summary>Retrieves the specified 32-bit (DWORD) value from the WNDCLASSEX structure associated with the specified window.</summary>
    /// <param name="hWnd">
    /// <para>Type: <b>HWND</b> A handle to the window and, indirectly, the class to which the window belongs.</para>
    /// <para><see href="https://docs.microsoft.com/windows/win32/api//winuser/nf-winuser-getclasslongw#parameters">Read more on docs.microsoft.com</see>.</para>
    /// </param>
    /// <param name="nIndex">Type: <b>int</b></param>
    /// <returns>
    /// <para>Type: <b>DWORD</b> If the function succeeds, the return value is the requested value. If the function fails, the return value is zero. To get extended error information, call <a href="/windows/desktop/api/errhandlingapi/nf-errhandlingapi-getlasterror">GetLastError</a>.</para>
    /// </returns>
    /// <remarks>
    /// <para><see href="https://docs.microsoft.com/windows/win32/api//winuser/nf-winuser-getclasslongw">Learn more about this API from docs.microsoft.com</see>.</para>
    /// </remarks>
    [DllImport("USER32.dll", ExactSpelling = true, EntryPoint = "GetClassLongPtrW", SetLastError = true)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
#if NET5_0_OR_GREATER
    [SupportedOSPlatform("windows5.0")]
#endif
    internal static extern nuint GetClassLongPtr(HWND hWnd, GET_CLASS_LONG_INDEX nIndex);

    internal static nuint GetClassLongAuto(HWND hwnd, WindowClassLongIndex nIndex)
        => AssemblyInfo.Is64Bit ?
            GetClassLongPtr(hwnd, (GET_CLASS_LONG_INDEX)nIndex) :
            GetClassLong(hwnd, (GET_CLASS_LONG_INDEX)nIndex);

    /// <summary>Replaces the specified value at the specified offset in the extra class memory or the WNDCLASSEX structure for the class to which the specified window belongs. (Unicode)</summary>
    /// <param name="hWnd">
    /// <para>Type: <b>HWND</b> A handle to the window and, indirectly, the class to which the window belongs.</para>
    /// <para><see href="https://docs.microsoft.com/windows/win32/api/winuser/nf-winuser-setclasslongptrw#parameters">Read more on docs.microsoft.com</see>.</para>
    /// </param>
    /// <param name="nIndex">Type: <b>int</b></param>
    /// <param name="dwNewLong">
    /// <para>Type: <b>LONG_PTR</b> The replacement value.</para>
    /// <para><see href="https://docs.microsoft.com/windows/win32/api/winuser/nf-winuser-setclasslongptrw#parameters">Read more on docs.microsoft.com</see>.</para>
    /// </param>
    /// <returns>
    /// <para>Type: <b>ULONG_PTR</b> If the function succeeds, the return value is the previous value of the specified offset. If this was not previously set, the return value is zero. If the function fails, the return value is zero. To get extended error information, call <a href="https://docs.microsoft.com/windows/desktop/api/errhandlingapi/nf-errhandlingapi-getlasterror">GetLastError</a>.</para>
    /// </returns>
    /// <remarks>
    /// <para>If you use the <b>SetClassLongPtr</b> function and the <b>GCLP_WNDPROC</b> index to replace the window procedure, the window procedure must conform to the guidelines specified in the description of the <a href="https://docs.microsoft.com/previous-versions/windows/desktop/legacy/ms633573(v=vs.85)">WindowProc</a> callback function. Calling <b>SetClassLongPtr</b> with the <b>GCLP_WNDPROC</b> index creates a subclass of the window class that affects all windows subsequently created with the class. An application can subclass a system class, but should not subclass a window class created by another process. Reserve extra class memory by specifying a nonzero value in the <b>cbClsExtra</b> member of the <a href="https://docs.microsoft.com/windows/desktop/api/winuser/ns-winuser-wndclassexa">WNDCLASSEX</a> structure used with the <a href="https://docs.microsoft.com/windows/desktop/api/winuser/nf-winuser-registerclassexa">RegisterClassEx</a> function. Use the <b>SetClassLongPtr</b> function with care. For example, it is possible to change the background color for a class by using <b>SetClassLongPtr</b>, but this change does not immediately repaint all windows belonging to the class.</para>
    /// <para>> [!NOTE] > The winuser.h header defines SetClassLongPtr as an alias which automatically selects the ANSI or Unicode version of this function based on the definition of the UNICODE preprocessor constant. Mixing usage of the encoding-neutral alias with code that not encoding-neutral can lead to mismatches that result in compilation or runtime errors. For more information, see [Conventions for Function Prototypes](/windows/win32/intl/conventions-for-function-prototypes).</para>
    /// <para><see href="https://docs.microsoft.com/windows/win32/api/winuser/nf-winuser-setclasslongptrw#">Read more on docs.microsoft.com</see>.</para>
    /// </remarks>
    [DllImport("USER32.dll", ExactSpelling = true, EntryPoint = "SetClassLongPtrW", SetLastError = true)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
#if NET5_0_OR_GREATER
    [SupportedOSPlatform("windows5.0")]
#endif
    internal static extern nuint SetClassLongPtr(HWND hWnd, GET_CLASS_LONG_INDEX nIndex, nuint dwNewLong);

    internal static nuint SetClassLongAuto(HWND hwnd, WindowClassLongIndex nIndex, nuint Value)
        => AssemblyInfo.Is64Bit ?
            SetClassLongPtr(hwnd, (GET_CLASS_LONG_INDEX)nIndex, Value) :
            SetClassLong(hwnd, (GET_CLASS_LONG_INDEX)nIndex, (int)Value);

    internal const int SC_MOUSEMOVE = 0xf012;
    internal const int SC_MOUSEMENU = 0xf090;

    [DllImport("Oleacc.dll", ExactSpelling = true)]
    internal static extern HANDLE GetProcessHandleFromHwnd(HWND hwnd);

    //[DllImport("gdiplus.dll", ExactSpelling = true)]
    //internal static extern int GdipCreateBitmapFromHICON(HICON hicon, out HBITMAP bitmap);

}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using Windows.Win32.Foundation;

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
    [SupportedOSPlatform("windows5.0")]
    public static extern IntPtr SetWindowLongPtr(HWND hWnd, UI.WindowsAndMessaging.WINDOW_LONG_PTR_INDEX nIndex, IntPtr dwNewLong);

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
    [SupportedOSPlatform("windows5.0")]
    public static extern nint GetClassLongPtr(HWND hWnd, UI.WindowsAndMessaging.GET_CLASS_LONG_INDEX nIndex);
}

public enum DWM_SYSTEMBACKDROP_TYPE : uint
{
    DWMSBT_AUTO = 0,
    DWMSBT_NONE = 1,
    DWMSBT_MAINWINDOW = 2,
    DWMSBT_TRANSIENTWINDOW = 3,
    DWMSBT_TABBEDWINDOW = 4
}
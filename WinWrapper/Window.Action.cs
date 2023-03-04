using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Windows.Win32;
using Windows.Win32.Graphics.Gdi;
using Windows.Win32.Storage.Xps;
using Windows.Win32.Foundation;
using Windows.Win32.UI.WindowsAndMessaging;
using Windows.Win32.UI.Shell;
using System;
using System.Drawing.Imaging;
using System.Drawing;
using System.Reflection.Metadata;
using Windows.Win32.System.Com.StructuredStorage;
using System.Runtime.Versioning;
using Windows.UI.Composition;

namespace WinWrapper;

partial struct Window
{
    /// <summary>
    /// Send the <see cref="PInvoke.WM_CLOSE"/> signel to the target <see cref="Window"/>
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void TryClose() => PInvoke.SendMessage(Handle, PInvoke.WM_CLOSE, 0, 0);

    /// <summary>
    /// Send the <see cref="PInvoke.WM_CLOSE"/> signel to the target <see cref="Window"/>
    /// </summary>
    public async Task TryCloseAsync() => await Task.Run(TryClose);

    /// <summary>
    /// Focus the <see cref="Window"/>
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Focus()
        => PInvoke.SetFocus(Handle);

    /// <summary>
    /// Activate the <see cref="Window"/>
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Activate()
        => PInvoke.SetActiveWindow(Handle);

    /// <summary>
    /// Redraw the <see cref="Window"/>
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Redraw()
        => PInvoke.RedrawWindow(Handle, null, null, Windows.Win32.Graphics.Gdi.REDRAW_WINDOW_FLAGS.RDW_INVALIDATE);

    /// <summary>
    /// Shows the <see cref="Window"/>
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void SetVisibility(bool Visibility) => IsVisible = Visibility;

    /// <summary>
    /// Shows the <see cref="Window"/>
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public async Task SetVisibilityAsync(bool Visibility) => await Task.Run(Visibility ? Show : Hide);

    /// <summary>
    /// Shows the <see cref="Window"/>
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Show() => IsVisible = true;

    /// <summary>
    /// Hides the <see cref="Window"/>
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Hide() => IsVisible = false;

    /// <summary>
    /// Minimizes the <see cref="Window"/>
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Minimize() => PInvoke.ShowWindow(Handle, SHOW_WINDOW_CMD.SW_MINIMIZE);

    /// <summary>
    /// Minimizes the <see cref="Window"/>
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Restore() => PInvoke.ShowWindow(Handle, SHOW_WINDOW_CMD.SW_RESTORE);


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void SendMessage(uint Message, WPARAM wPARAM, LPARAM lPARAM)
        => PInvoke.SendMessage(Handle, Message, wPARAM, lPARAM);


    public delegate LRESULT WndProcOverride(HWND Handle, uint code, WPARAM wParam, LPARAM lParam, WNDPROC Original);


    public Bitmap PrintWindow()
    {
        // Refernce: https://stackoverflow.com/questions/891345/get-a-screenshot-of-a-specific-application
        var rc = Bounds;

        Bitmap bmp = new(rc.Width, rc.Height, PixelFormat.Format32bppArgb);
        Graphics gfxBmp = Graphics.FromImage(bmp);
        IntPtr hdcBitmap = gfxBmp.GetHdc();

        PInvoke.PrintWindow(Handle, new HDC(hdcBitmap), 0);

        gfxBmp.ReleaseHdc(hdcBitmap);
        gfxBmp.Dispose();

        return bmp;
    }

    public unsafe void SetAppId(string AppId)
    {
        Guid iid = new("886D8EEB-8CF2-4446-8D02-CDBA1DBDCF99");
        int result1 = SHGetPropertyStoreForWindow(Handle, ref iid, out IPropertyStore prop);
        // Name = System.AppUserModel.ID
        // ShellPKey = PKEY_AppUserModel_ID
        // FormatID = 9F4C2855-9F79-4B39-A8D0-E1D42DE1D5F3
        // PropID = 5
        // Type = String (VT_LPWSTR)
        PropertyKey AppUserModelIDKey = new("{9F4C2855-9F79-4B39-A8D0-E1D42DE1D5F3}", 5);

        PropVariant pv = new(AppId);

        uint result2 = prop.SetValue(ref AppUserModelIDKey, pv);

        Marshal.ReleaseComObject(prop);
    }

    /// <summary>
    /// Apply Drag Move. Call this only if user's primary mouse cursor is down
    /// </summary>
    public void DragMove()
    {
        Cursor.ReleaseCapture();
        PInvoke.SendMessage(Handle, PInvoke.WM_LBUTTONUP, default, default);
        PInvoke.SendMessage(Handle, PInvoke.WM_SYSCOMMAND, new(PInvoke.SC_MOUSEMOVE), default);
    }

    /// <summary>
    /// Apply Drag Move. Call this only if user's primary mouse cursor is down
    /// </summary>
    public void DragMoveRightClick()
    {
        PInvoke.SendMessage(Handle, PInvoke.WM_SYSCOMMAND, new(PInvoke.SC_MOUSEMENU), default);
        //PInvoke.SendMessage(Handle, PInvoke.WM_RBUTTONUP, default, default);
    }

    ///// <summary>
    ///// Minimizes the <see cref="Window"/>
    ///// </summary>
    //public void OverrideWndProc(WndProcOverride NewWndProc)
    //{
    //    WNDPROC? Original = default;
    //    try
    //    {
    //        GC.KeepAlive(NewWndProc);
    //        LRESULT newwndproc(HWND Handle, uint code, WPARAM wParam, LPARAM lParam) => NewWndProc.Invoke(Handle, code, wParam, lParam, Original!);
    //        WNDPROC newwndproce = newwndproc;
    //        GC.KeepAlive(newwndproce);
    //        var ptr = PInvoke.SetWindowLongPtr(Handle, WINDOW_LONG_PTR_INDEX.GWLP_WNDPROC, Marshal.GetFunctionPointerForDelegate(newwndproce));
    //        Original = Marshal.GetDelegateForFunctionPointer<WNDPROC>((nint)ptr);
    //        GC.KeepAlive(Original);
    //    } catch
    //    {
    //    }
    //}
    //public void SetSubclass(SUBCLASSPROC s, uint Id)
    //    => PInvoke.SetWindowSubclass(Handle, s, Id, UIntPtr.Zero);
    [DllImport("shell32.dll")]
    private static extern int SHGetPropertyStoreForWindow(
        HWND hwnd,
        ref Guid iid /*IID_IPropertyStore*/,
        [Out(), MarshalAs(UnmanagedType.Interface)] out IPropertyStore propertyStore);
    // https://emoacht.wordpress.com/2012/11/14/csharp-appusermodelid/
    // IPropertyStore Interface
    [ComImport,
        InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
        Guid("886D8EEB-8CF2-4446-8D02-CDBA1DBDCF99")]
    private interface IPropertyStore
    {
        uint GetCount([Out] out uint cProps);
        uint GetAt([In] uint iProp, out PropertyKey pkey);
        uint GetValue([In] ref PropertyKey key, [Out] PropVariant pv);
        uint SetValue([In] ref PropertyKey key, [In] PropVariant pv);
        uint Commit();
    }


    // PropertyKey Structure
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct PropertyKey
    {
        private Guid formatId;    // Unique GUID for property
        private Int32 propertyId; // Property identifier (PID)

        public Guid FormatId
        {
            get
            {
                return formatId;
            }
        }

        public Int32 PropertyId
        {
            get
            {
                return propertyId;
            }
        }

        public PropertyKey(Guid formatId, Int32 propertyId)
        {
            this.formatId = formatId;
            this.propertyId = propertyId;
        }

        public PropertyKey(string formatId, Int32 propertyId)
        {
            this.formatId = new Guid(formatId);
            this.propertyId = propertyId;
        }

    }


    // PropVariant Class (only for string value)
    [StructLayout(LayoutKind.Explicit)]
    public class PropVariant : IDisposable
    {
        [FieldOffset(0)]
        ushort valueType;     // Value type

        // [FieldOffset(2)]
        // ushort wReserved1; // Reserved field
        // [FieldOffset(4)]
        // ushort wReserved2; // Reserved field
        // [FieldOffset(6)]
        // ushort wReserved3; // Reserved field

        [FieldOffset(8)]
        IntPtr ptr;           // Value


        // Value type (System.Runtime.InteropServices.VarEnum)
        public VarEnum VarType
        {
            get { return (VarEnum)valueType; }
            set { valueType = (ushort)value; }
        }

        public bool IsNullOrEmpty
        {
            get
            {
                return (valueType == (ushort)VarEnum.VT_EMPTY ||
                        valueType == (ushort)VarEnum.VT_NULL);
            }
        }

        // Value (only for string value)
        public string? Value
        {
            get
            {
                return Marshal.PtrToStringUni(ptr);
            }
        }


        public PropVariant()
        { }

        public PropVariant(string value)
        {
            if (value == null)
                throw new ArgumentException("Failed to set value.");

            valueType = (ushort)VarEnum.VT_LPWSTR;
            ptr = Marshal.StringToCoTaskMemUni(value);
        }

        ~PropVariant()
        {
            Dispose();
        }

        public void Dispose()
        {
            PropVariantClear(this);
            GC.SuppressFinalize(this);
        }
        [DllImport("OLE32.dll", ExactSpelling = true)]
        [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
        [SupportedOSPlatform("windows5.0")]
        private extern static void PropVariantClear([In, Out] PropVariant pvar);
    }
}
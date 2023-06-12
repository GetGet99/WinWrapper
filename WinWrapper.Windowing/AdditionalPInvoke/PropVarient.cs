using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;

namespace WinWrapper.Windowing.AdditionalPInvoke;

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
        try
        {
            PropVariantClear(this);
        } catch { }
        GC.SuppressFinalize(this);
    }
    [DllImport("OLE32.dll", ExactSpelling = true)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
#if NET5_0_OR_GREATER
    [SupportedOSPlatform("windows5.0")]
#endif
    private extern static void PropVariantClear([In, Out] PropVariant pvar);

}

using System.Runtime.InteropServices;

namespace WinWrapper.Windowing.AdditionalPInvoke;

// https://emoacht.wordpress.com/2012/11/14/csharp-appusermodelid/
// IPropertyStore Interface
[ComImport,
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
    Guid("886D8EEB-8CF2-4446-8D02-CDBA1DBDCF99")]
interface IPropertyStore
{
    uint GetCount([Out] out uint cProps);
    uint GetAt([In] uint iProp, out PropertyKey pkey);
    uint GetValue([In] ref PropertyKey key, [Out] PropVariant pv);
    uint SetValue([In] ref PropertyKey key, [In] PropVariant pv);
    uint Commit();
}
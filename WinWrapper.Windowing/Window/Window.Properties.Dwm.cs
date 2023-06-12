using Windows.Win32;
using Windows.Win32.UI.WindowsAndMessaging;
using Windows.Win32.Graphics.Gdi;
using Windows.Win32.Foundation;
namespace WinWrapper.Windowing;

partial struct Window
{

    public unsafe void SetLayeredWindowBitmap(nint HBitmap, byte Opacity = 255)
    {
        var Bitmap = new HBITMAP(HBitmap);
        var ScreenHDC = PInvoke.GetDC(default);
        var MemHDC = PInvoke.CreateCompatibleDC(ScreenHDC);
        HBITMAP OldBitmap = default;

        try
        {
            OldBitmap = new(PInvoke.SelectObject(new HDC(MemHDC.Value), new HGDIOBJ(Bitmap.Value)).Value);
            BLENDFUNCTION blend = new()
            {
                BlendOp = (byte)PInvoke.AC_SRC_OVER,
                BlendFlags = 0,
                SourceConstantAlpha = Opacity,
                AlphaFormat = (byte)PInvoke.AC_SRC_ALPHA
            };
            PInvoke.UpdateLayeredWindow(
                hWnd: HWND,
                hdcDst: ScreenHDC,
                pptDst: null,
                psize: null,
                hdcSrc: new HDC(MemHDC.Value),
                pptSrc: null,
                crKey: new COLORREF(0),
                pblend: &blend,
                dwFlags: UPDATE_LAYERED_WINDOW_FLAGS.ULW_ALPHA
            );
        }
        finally
        {
            PInvoke.ReleaseDC(default, ScreenHDC);
            if (!Bitmap.IsNull)
                PInvoke.SelectObject(new HDC(MemHDC.Value), new HGDIOBJ(OldBitmap.Value));
        }
    }
}
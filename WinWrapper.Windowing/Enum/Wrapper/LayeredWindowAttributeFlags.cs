using Windows.Win32.UI.WindowsAndMessaging;
namespace WinWrapper.Windowing;
[Flags]
public enum LayeredWindowAttributeFlags : uint
{
    Alpha = LAYERED_WINDOW_ATTRIBUTES_FLAGS.LWA_ALPHA,
    ColorKey = LAYERED_WINDOW_ATTRIBUTES_FLAGS.LWA_COLORKEY,
}

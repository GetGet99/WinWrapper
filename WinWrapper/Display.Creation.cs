using System.Drawing;
using System.Runtime.CompilerServices;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.Graphics.Gdi;
namespace WinWrapper;

partial struct Display
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Display FromPoint(Point Location)
        => new(PInvoke.MonitorFromPoint(Location, MONITOR_FROM_FLAGS.MONITOR_DEFAULTTOPRIMARY));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Display FromRectangle(Rectangle Location)
        => new(PInvoke.MonitorFromRect(Location, MONITOR_FROM_FLAGS.MONITOR_DEFAULTTOPRIMARY));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static Display FromHandle(HMONITOR Handle)
        => new(Handle);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Display FromHandle(nint Handle)
        => new((HMONITOR)Handle);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static List<Display> GetAllMonitors()
        => GetMonitorAPI.EnumMonitors();
}
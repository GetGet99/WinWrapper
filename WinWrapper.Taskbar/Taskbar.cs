using System.Runtime.InteropServices;
using Windows.Win32.UI.Shell;
using WinWrapper.Windowing;

namespace WinWrapper.Taskbar;

[ComImport()]
[Guid("56fdf344-fd6d-11d0-958a-006097c9a090")]
[ClassInterface(ClassInterfaceType.None)]
class TaskbarInstance
{

}
public static class Taskbar
{
    static TaskbarInstance TaskbarInstance = Activator.CreateInstance<TaskbarInstance>();
    static ITaskbarList3 ITaskbarList3 = (ITaskbarList3)TaskbarInstance;

    public static unsafe bool SetOverlayIcon(Window window, Icon Icon, string AlternateText)
    {
        try
        {
            fixed (char* alternateText = AlternateText)
            {
                ITaskbarList3.SetOverlayIcon(new(window.Handle), new(Icon.Handle), alternateText);
            }
        }
        catch (Exception)
        {
            return false;
        }//sometimes this api call may timeout and throw
        return true;
    }
}
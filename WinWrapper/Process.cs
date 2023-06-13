using Windows.Win32;
using Windows.Win32.Foundation;
using SysDiaProcess = System.Diagnostics.Process;
namespace WinWrapper;

public struct Process
{
    readonly HANDLE Handle;
    private Process(HANDLE ProcessHandle)
    {
        Handle = ProcessHandle;
    }
    internal static Process FromHandle(HANDLE ProcessHandle) => new(ProcessHandle);
    public static Process FromHandle(nint ProcessHandle) => new((HANDLE)ProcessHandle);
    public readonly uint Id => PInvoke.GetProcessId(Handle);
    public readonly SysDiaProcess GetDotNetProcess => SysDiaProcess.GetProcessById((int)Id);

    public static Process Current => new(PInvoke.GetCurrentProcess());
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    public static Process FromHandle(HANDLE ProcessHandle) => new(ProcessHandle);
    public uint Id => PInvoke.GetProcessId(Handle);
    public SysDiaProcess GetDotNetProcess => SysDiaProcess.GetProcessById((int)Id);
}

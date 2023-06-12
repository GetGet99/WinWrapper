using System;
using System.Collections.Generic;
using System.Text;

namespace WinWrapper;

public class AssemblyInfo
{
    public unsafe static bool Is64Bit = sizeof(nint) == 8;
}

using System;
using System.Collections.Generic;
using System.Text;
using Windows.Win32;

namespace WinWrapper.Windowing;

public static class WindowMessagesHelper
{
    public static WindowMessages Register(string Name)
        => (WindowMessages)PInvoke.RegisterWindowMessage(Name);
}

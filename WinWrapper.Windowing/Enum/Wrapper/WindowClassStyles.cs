﻿using Windows.Win32.UI.WindowsAndMessaging;
namespace WinWrapper;
[Flags]
public enum WindowClassStyles : uint
{
    CS_VREDRAW = WNDCLASS_STYLES.CS_VREDRAW,
    CS_HREDRAW = WNDCLASS_STYLES.CS_HREDRAW,
    CS_DBLCLKS = WNDCLASS_STYLES.CS_DBLCLKS,
    CS_OWNDC = WNDCLASS_STYLES.CS_OWNDC,
    CS_CLASSDC = WNDCLASS_STYLES.CS_CLASSDC,
    CS_PARENTDC = WNDCLASS_STYLES.CS_PARENTDC,
    CS_NOCLOSE = WNDCLASS_STYLES.CS_NOCLOSE,
    CS_SAVEBITS = WNDCLASS_STYLES.CS_SAVEBITS,
    CS_BYTEALIGNCLIENT = WNDCLASS_STYLES.CS_BYTEALIGNCLIENT,
    CS_BYTEALIGNWINDOW = WNDCLASS_STYLES.CS_BYTEALIGNWINDOW,
    CS_GLOBALCLASS = WNDCLASS_STYLES.CS_GLOBALCLASS,
    CS_IME = WNDCLASS_STYLES.CS_IME,
    CS_DROPSHADOW = WNDCLASS_STYLES.CS_DROPSHADOW,
}
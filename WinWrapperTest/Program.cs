using WinWrapper;
using WinWrapper.Windowing;

var window = Window.ForegroundWindow;

window.Styling.Style |= WindowStyles.Border;
// or
window[WindowStyles.Border] = false;
// or
window.Styling.StyleFlag[WindowStyles.Border] = true;
// or
window.Styling[WindowStyles.Border] = true;
// or
window[WindowStyles.Border] = true;

window.Styling.ExStyle |= WindowExStyles.Transparent;
// or
window.Styling.ExStyleFlag[WindowExStyles.Transparent] = true;
// or
window.Styling[WindowExStyles.Transparent] = true;
// or
window[WindowExStyles.Transparent] = true;

window.Styling.ExStyle &= ~WindowExStyles.Transparent;
// or
window[WindowExStyles.Transparent] = true;
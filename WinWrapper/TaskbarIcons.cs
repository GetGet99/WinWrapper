//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Windows.Win32.UI.WindowsAndMessaging;
//namespace WinWrapper
//{
//    class TaskbarIcons
//    {
//        public void GetIcons()
//        {
//            var windows =
//                from x in Window.GetAllWindows()
//                where x.IsVisible &&
//                (
//                    x.ExStyle.HasFlag(WINDOW_EX_STYLE.WS_EX_APPWINDOW) ||
//                    !x.ExStyle.HasFlag(WINDOW_EX_STYLE.WS_EX_TOOLWINDOW)
//                )
//                select x;
//            foreach (var window in windows)
//            {
//                WindowMessageMonitor windowMessageMonitor = new(window);
//                windowMessageMonitor.WindowMessageReceived += WindowMessageMonitor_WindowMessageReceived;
//            }
//        }

//        private Windows.Win32.Foundation.LRESULT? WindowMessageMonitor_WindowMessageReceived(Windows.Win32.Foundation.HWND hWnd, uint uMsg, Windows.Win32.Foundation.WPARAM wParam, Windows.Win32.Foundation.LPARAM lParam)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}

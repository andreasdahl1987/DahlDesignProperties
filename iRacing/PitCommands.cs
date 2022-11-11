using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace User.PluginSdkDemo
{
    class PitCommands
    {
        [DllImport("user32.dll")]
        public static extern IntPtr PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        private const int SW_SHOWMAXIMIZED = 3;

        public static void iRacingChat(string text)
        {
            const uint WM_KEYDOWN = 0x100;
            const Int32 WM_CHAR = 0x0102;
            const Int32 WM_KEYUP = 0x0101;
            Process p = Process.GetProcessesByName("iRacingSim64DX11").FirstOrDefault(); //iRacingSim64DX11
            if (p != null)
            {
                IntPtr windowHandle = (IntPtr)p.MainWindowHandle;
                char[] chars = text.ToCharArray();
                PostMessage(windowHandle, WM_KEYDOWN, (IntPtr)Keys.T, (IntPtr)0);
                PostMessage(windowHandle, WM_KEYUP, (IntPtr)Keys.T, (IntPtr)0);
                Thread.Sleep(50);
                for (int i = 0; i < chars.Count(); i++) { PostMessage(windowHandle, WM_CHAR, (IntPtr)chars[i], (IntPtr)0); }
                PostMessage(windowHandle, WM_KEYDOWN, (IntPtr)Keys.Enter, (IntPtr)0);
                PostMessage(windowHandle, WM_KEYUP, (IntPtr)Keys.Enter, (IntPtr)0);
            }
        }

        public static void Fullscreen()
        {
            Process iRacing = Process.GetProcessesByName("iRacingSim64DX11").FirstOrDefault(); //iRacingSim64DX11
            if (iRacing != null)
            {
                ShowWindow(iRacing.MainWindowHandle, SW_SHOWMAXIMIZED);
            } 
        }




    }
}

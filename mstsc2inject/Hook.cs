// Michael Wollensack - 16.08.2026

using System;
using System.Runtime.InteropServices;
using System.Threading;
using EasyHook;

namespace mstsc2
{
    public class HookEntryPoint : IEntryPoint
    {
        public HookEntryPoint(
            RemoteHooking.IContext context,
            uint dpi)
        {
            UserDpi = dpi;
        }

        public void Run(
            RemoteHooking.IContext context,
            uint dpi)
        {
            UserDpi = dpi;

            var hookGetDpiForMonitor = LocalHook.Create(
                LocalHook.GetProcAddress(
                    "shcore.dll",
                    "GetDpiForMonitor"),
                new GetDpiForMonitorDelegate(
                    GetDpiForMonitorHook),
            null);

            hookGetDpiForMonitor.ThreadACL.SetExclusiveACL(new int[0]);

            while (true)
            {
                Thread.Sleep(1000);
            }
        }

        static uint UserDpi = 96;

        [UnmanagedFunctionPointer(CallingConvention.StdCall, SetLastError = true)]
        delegate int GetDpiForMonitorDelegate(IntPtr hMonitor, int dpiType, out uint dpiX, out uint dpiY);

        // shcore!GetDpiForMonitor
        // https://learn.microsoft.com/en-us/windows/win32/api/shellscalingapi/nf-shellscalingapi-getdpiformonitor
        [DllImport("shcore.dll", SetLastError = true)]
        static extern int GetDpiForMonitor(IntPtr hMonitor, int dpiType, out uint dpiX, out uint dpiY);

        static int GetDpiForMonitorHook(IntPtr hMonitor, int dpiType, out uint dpiX, out uint dpiY)
        {
            int hr = GetDpiForMonitor(hMonitor, dpiType, out dpiX, out dpiY);
            //System.Windows.Forms.MessageBox.Show("Monitor DPI: " + dpiX + ", User DPI: " + UserDpi);
            dpiX = UserDpi;
            dpiY = UserDpi;
            return hr;
        }
    }
}

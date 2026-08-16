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
            RemoteHooking.IContext context)
        {
        }

        public void Run(
            RemoteHooking.IContext context)
        {
            var hookGetScaleFactorMonitor = LocalHook.Create(
                LocalHook.GetProcAddress(
                    "shcore.dll",
                    "GetScaleFactorForMonitor"),
                new GetScaleFactorForMonitorDelegate(
                    GetScaleFactorForMonitorHook),
                null);

            hookGetScaleFactorMonitor.ThreadACL.SetExclusiveACL(new int[0]);

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

        [UnmanagedFunctionPointer(CallingConvention.StdCall, SetLastError = true)]
        delegate int GetScaleFactorForMonitorDelegate(IntPtr hMonitor, out int scaleFactor);

        // shcore!GetScaleFactorForMonitor
        // https://learn.microsoft.com/en-us/windows/win32/api/shellscalingapi/nf-shellscalingapi-getscalefactorformonitor
        [DllImport("shcore.dll", SetLastError = true)]
        static extern int GetScaleFactorForMonitor(IntPtr hMonitor, out int scaleFactor);

        static int GetScaleFactorForMonitorHook(IntPtr hMonitor, out int scaleFactor)
        {
            int hr = GetScaleFactorForMonitor(hMonitor, out scaleFactor);
            System.Windows.Forms.MessageBox.Show("Scale Factor: " + scaleFactor);
            return hr;
        }


        [UnmanagedFunctionPointer(CallingConvention.StdCall, SetLastError = true)]
        delegate int GetDpiForMonitorDelegate(IntPtr hMonitor, int dpiType, out uint dpiX, out uint dpiY);

        [DllImport("shcore.dll", SetLastError = true)]
        static extern int GetDpiForMonitor(IntPtr hMonitor, int dpiType, out uint dpiX, out uint dpiY);

        static int GetDpiForMonitorHook(IntPtr hMonitor, int dpiType, out uint dpiX, out uint dpiY)
        {
            int hr = GetDpiForMonitor(hMonitor, dpiType, out dpiX, out dpiY);
            System.Windows.Forms.MessageBox.Show("DPI: " + dpiX);
            return hr;
        }
    }
}

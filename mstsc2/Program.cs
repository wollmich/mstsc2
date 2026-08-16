// Michael Wollensack 16.08.2026

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using EasyHook;

namespace mstsc2
{
    class Program
    {
        static void Main(string[] args)
        {
            uint dpi = 96;

            List<string> remaingArgs = new List<string>();
            foreach (string arg in args)
            {
                if (arg.StartsWith("/dpi:", StringComparison.OrdinalIgnoreCase))
                {
                    dpi = uint.Parse(arg.Substring(5));
                }
                else
                {
                    remaingArgs.Add(arg);
                }
            }
            
            string arguments = string.Join(" ", remaingArgs);

            var psi = new ProcessStartInfo
            {
                FileName = "mstsc.exe",
                Arguments = arguments
            };

            Process mstsc = Process.Start(psi);
            mstsc.WaitForInputIdle();

            Console.WriteLine("Started MSTSC PID "  + mstsc.Id);

            RemoteHooking.Inject(
                mstsc.Id,
                InjectionOptions.Default,
                @"mstsc2inject.dll",
                @"mstsc2inject.dll",
                dpi);

            Console.WriteLine("Inject DPI " + dpi);
        }
    }
}

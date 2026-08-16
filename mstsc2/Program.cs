// Michael Wollensack 16.08.2026

using System;
using System.Diagnostics;
using System.Reflection;
using EasyHook;

namespace mstsc2
{
    class Program
    {
        static void Main(string[] args)
        {
            string arguments = string.Join(" ", args);

            var psi = new ProcessStartInfo
            {
                FileName = "mstsc.exe",
                Arguments = arguments
            };

            Process mstsc = Process.Start(psi);

            Console.WriteLine("Started MSTSC PID "  + mstsc.Id);

            RemoteHooking.Inject(
                mstsc.Id,
                InjectionOptions.Default,
                @"mstsc2inject.dll",
                @"mstsc2inject.dll");
        }
    }
}

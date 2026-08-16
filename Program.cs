// Michael Wollensack 16.08.2026

using System;
using System.Diagnostics;

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
        }
    }
}

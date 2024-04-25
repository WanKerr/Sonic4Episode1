using System;
using System.Runtime.InteropServices;

namespace Sonic4Episode1.FNA
{
    static class MonoInternals
    {
        [DllImport("__Native")]
        internal static extern void mono_trace_enable(int enable);
        [DllImport("__Native")]
        internal static extern int mono_trace_set_options(string options);
    }

    class Program
    {
        static void Main(string[] args)
        {
            MonoInternals.mono_trace_enable(1);
            MonoInternals.mono_trace_set_options("E:all");

            using (var game = new Sonic4Ep1())
            {
                game.Run();
            }
        }
    }
}

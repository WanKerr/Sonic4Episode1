using System;

namespace Sonic4Episode1.Win32
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (var game = new Sonic4Ep1())
                game.Run();
        }
    }
#endif
}

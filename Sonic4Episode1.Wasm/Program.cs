using System;
using System.Runtime.InteropServices.JavaScript;
using System.Threading.Tasks;

public static partial class Program
{
    private static Sonic4Ep1 _game;

    internal static Task Main()
    {
        _game = new Sonic4Ep1();
        //_game.Run();
        SetMainLoop(MainLoop);
        return Task.Delay(-1);
    }


    private static void MainLoop()
    {
        try
        {
            _game?.RunOneFrame();
        }
        catch (Exception e)
        {
            Console.Error.WriteLine(e);
            throw;
        }
    }

    [JSImport("setMainLoop", "main.js")]
    internal static partial void SetMainLoop([JSMarshalAs<JSType.Function>] Action cb);
}
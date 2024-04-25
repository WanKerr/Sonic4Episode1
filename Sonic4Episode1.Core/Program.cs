namespace Sonic4Episode1.Core
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var sonic = new Sonic4Ep1())
            {
                sonic.Run();
            }
        }
    }
}

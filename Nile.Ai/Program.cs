using OpenTK.Windowing.Desktop;
using System;

namespace Nile.Ai
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var game = new Game(GameWindowSettings.Default, NativeWindowSettings.Default))
            {
                game.Run();
            }
        }
    }
}

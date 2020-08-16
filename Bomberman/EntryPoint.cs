using System;
using System.IO;
using System.Threading.Tasks;

namespace Bomberman
{
    internal class EntryPoint
    {
        private const string ServerUrl = "http://codingdojo.kz/codenjoy-contest/board/player/cb3unup1xxjszml2ubw8?code=3756093975220038158&gameName=bomberman";

        private static void Main()
        {
            if (File.Exists("log.txt"))
                File.Delete("log.txt");

            Console.SetWindowSize(Console.LargestWindowWidth - 3, Console.LargestWindowHeight - 3);

            var bot = new Solver(ServerUrl);

            Task.Run(bot.Play);

            Console.ReadKey();

            bot.InitiateExit();
        }
    }
}

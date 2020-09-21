using System;

namespace DiscordPugBot
{
    class Program
    {
        static void Main(string[] args)
        {
            var bot = new Bot();
            bot.Start().GetAwaiter().GetResult();
        }
    }
}

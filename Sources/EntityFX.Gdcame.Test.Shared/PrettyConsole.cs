using System;

namespace EntityFx.EconomicsArcade.Test.Shared
{
    internal static class PrettyConsole
    {
        private static readonly object LockObject = new { };

        public static void WriteColor(ConsoleColor color, string text)
        {
            WriteColor(color, text, null);
        }

        public static void WriteColor(ConsoleColor color, string text, params object[] args)
        {
            var fg = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.Write(text, args);
            Console.ForegroundColor = fg;
        }

        public static void WriteLineColor(ConsoleColor color, string text)
        {
            WriteLineColor(color, text, null);
        }

        public static void WriteLineColor(ConsoleColor color, string text, params object[] args)
        {
            var fg = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(text, args);
            Console.ForegroundColor = fg;
        }
    }
}
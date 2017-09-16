// In short apache 2 license, check LICENSE file for the legalese

using System;

namespace Lambda
{
    public static partial class Extensions
    {

        public static void WriteLine(this string str, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(str);
        }

        public static void Write(this string str, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write(str);
        }

        public static void OverwriteLine(this string str, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write('\r' + str);
        }

        public static void WriteLine(this string str)
        {
            WriteLine(str, ConsoleColor.White);
        }

        public static void Write(this string str)
        {
            Write(str, ConsoleColor.White);
        }

        public static void OverwriteLine(this string str)
        {
            OverwriteLine(str, ConsoleColor.White);
        }
    }
}

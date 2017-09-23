// In short apache 2 license, check LICENSE file for the legalese

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace Lambda
{
    public partial class Utilities
    {
        public enum Operations
        {
            Sum,
            Sub,
            Mul,
            Div
        }

        public struct RangeConfig
        {
            public int Start { get; set; }
            public int Length { get; set; }
            public int Interval { get; set; }
            public Operations Op { get; set; }
        }

        public static int[] Range(int start, int length, int interval, Operations op)
        {
            if (interval == 0) throw new ArgumentException("Stepping by 0 would create an infinite loop");
            var work = new List<int>();
            switch (op)
            {
                case Operations.Sum:
                    if (start > length) throw new ArgumentException("Can't have a start bigger than lenght");
                    for (var i = start; i <= length; i += interval)
                    {
                        work.Add(i);
                    }
                    break;

                case Operations.Sub:
                    for (var i = start; i <= length; i -= interval)
                    {
                        work.Add(i);
                    }
                    break;

                case Operations.Mul:
                    if (start > length) throw new ArgumentException("Can't have a start bigger than lenght");
                    if (start == 0) throw new ArgumentException("Can't multiply by 0");
                    for (var i = start; i <= length; i *= interval)
                    {
                        work.Add(i);
                    }
                    break;

                case Operations.Div:
                    if (start == 0) throw new ArgumentException("Can't divide by 0");
                    for (var i = start; i <= length; i /= interval)
                    {
                        work.Add(i);
                    }
                    break;

                default:
                    break;
            }
            return work.ToArray();
        }

        public static int[] Range(RangeConfig config) => Range(config.Start, config.Length, config.Interval, config.Op);

        public static int[] Range(int length) => Range(0, length, 1, Operations.Sum);

        public static int[] Range(int start, int length, int interval) => Range(start, length, interval, Operations.Sum);

        public static string ConsolePassword(string prompt, string mask, ConsoleColor color = ConsoleColor.White)
        {
            prompt.Write(color);
            
            var password = "";
            ConsoleKeyInfo key;

            do
            {
                key = Console.ReadKey(true);

                if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                {
                    password += key.KeyChar;
                    mask.Write();
                }
                else
                {
                    if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                    {
                        password = password.Substring(0, (password.Length - 1));
                        "\b \b".Write();
                    }
                }
            } while (key.Key != ConsoleKey.Enter);
            "\n".Write();
            return password;
        }

        public static void SetupConsole(ConsoleColor bg, ConsoleColor fg, string title, bool clear = false)
        {
            Console.BackgroundColor = bg;
            Console.ForegroundColor = fg;
            Console.Title = title;
            if (clear) Console.Clear();
        }

        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("Local IP Address Not Found!");
        }
    }
}
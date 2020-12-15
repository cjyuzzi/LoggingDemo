using System;
using System.Diagnostics;

namespace DebuggerDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var categories = new string[] { "國文", "英文", "數學", "自然", "社會" };

            for (int i = 0; i < 5; i++)
            {
                var category = categories[i];
                var condition = true;

                // 少用
                Debugger.Log(i, category, $"This is a debug message using Debbuger.\n");

                // 常用
                Debug.Write($"[{category}]");
                Debug.WriteLine(" This is a debug message.");
                Debug.WriteIf(condition, $"[True]");
                Debug.WriteLineIf(condition, "Writig line if true.");
            }

            Console.ReadKey();
        }
    }
}

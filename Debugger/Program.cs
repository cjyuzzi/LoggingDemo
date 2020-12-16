using System;
using System.Diagnostics;

namespace DebuggerDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var categories = new string[] { "國文", "英文", "數學", "自然", "社會" };

            // 靜態類別 Debugger 是與偵錯器進行通訊的媒介，可利用它人為地啟動偵錯器、觸發中斷點或是使用 Debugger.Log() 紀錄訊息到偵錯器
            #region Debugger

            // 可判斷偵錯器是否有啟動
            if (!Debugger.IsAttached)
            {

                // 手動啟動偵錯器（JIT Debugger）
                //Debugger.Launch();
            }

            // 手動觸發中斷點
            Debugger.Break();

            // 檢查紀錄日誌功能是否開啟
            if (Debugger.IsLogging())
            {
                // 向偵錯器紀錄一筆訊息，少用。預設 DefaultCategory 為 Null
                Debugger.Log(1, Debugger.DefaultCategory, $"This is a debug message from Debbuger.\n");
            }

            #endregion

            // 靜態類別 Debug 能執行與偵錯相關的操作，並且這些操作程式碼只會在 DEBUG 模式下才會進行編譯，建議使用！
            #region Debug

            for (int i = 0; i < 5; i++)
            {
                var category = categories[i];
                var condition = true;

                // 常用的紀錄方法：
                Debug.Write($"[{category}]");
                Debug.WriteLine(" This is a debug message.");

                // 可加入判斷條件
                Debug.WriteIf(condition, $"[True]");
                Debug.WriteLineIf(condition, "Write line if true.");

                // 斷言：用於確認條件是否如預期般執行，如果不符合，程式會暫停進入中斷模式，並顯示訊息
                Debug.Assert(categories.Length > 10);
            }

            #endregion

            Console.ReadKey();
        }
    }
}

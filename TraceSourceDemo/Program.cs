using System;
using System.Diagnostics;

namespace TraceSourceDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // SourceLevels：需要被記錄下來的最低紀錄檔事件等級。可嘗試指定不同的 SourceLevels 觀察差異。
            var source = new TraceSource("Twice", SourceLevels.Warning);

            // 在所有事件等級都會被紀錄的條件下，觀察在不同的 SourceLevels 會有何差異。
            TraceEventAllLevels(source);

            // 觀察 TraceSource 預設有哪些監聽器（Listener）。
            Console.Write("預設");
            PrintListners(source);

            // 註冊自訂的輸出通道 ConsoleTraceListener 監聽器。
            source.Listeners.Add(new ConsoleTraceListener());

            PrintListners(source);

            // 觀察 ConsoleTraceListener 是否有紀錄到 Console。
            TraceEventAllLevels(source);

            Console.ReadKey();
        }

        private static void TraceEventAllLevels(TraceSource source)
        {
            var eventID = 1;

            // TraceEventType：紀錄檔事件的等級。這裡取得所有等級來觀察差異。
            var eventTypes = (TraceEventType[])Enum.GetValues(typeof(TraceEventType));

            for (int i = 0; i < eventTypes.Length; i++)
            {
                var eventType = eventTypes[i];
                source.TraceEvent(eventType, eventID++, $"This is a {eventType} message.");
            }
        }

        private static void PrintListners(TraceSource source)
        {
            Console.WriteLine($"監聽器有 {source.Listeners.Count} 個：");

            foreach (TraceListener listener in source.Listeners)
            {
                Console.WriteLine($"\t{listener.GetType()}");
            }
        }
    }
}

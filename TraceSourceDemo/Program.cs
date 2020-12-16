using System;
using System.Diagnostics;

namespace TraceSourceDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // 指定名稱與最低紀錄等級建立 TraceSource 物件。
            // SourceLevels：指定需要紀錄事件的最低等級，可嘗試指定不同的 SourceLevels 觀察差異。
            var source = new TraceSource("Twice", SourceLevels.Warning);

            Console.WriteLine("TraceSource created.");

            ShowSourceSwitch(source);

            // 在所有事件等級都會被紀錄的條件下，觀察在不同的 SourceLevels 會有何差異。
            TraceEventAllLevels(source);

            // 觀察 TraceSource 預設有哪些監聽器（Listener）。
            Console.Write("預設");
            PrintListners(source);

            // 註冊自訂的輸出通道 ConsoleTraceListener 監聽器。
            var console = new ConsoleTraceListener()
            {
                // TraceFilter: 會過濾來自於 TraceSource 的紀錄訊息。系統預設提供：EventTypeFilter 與 SourceFilter。
                Filter = new SourceFilter("Twice"),
                // TraceOptions: 決定實際要輸出哪些上下文資訊。（該列舉標記了 FlagsAttribute，因此可以任意組合）
                TraceOutputOptions = TraceOptions.ThreadId | TraceOptions.DateTime,

                // 縮排設定
                IndentLevel = 5,
                IndentSize = 5
            };
            source.Listeners.Add(console);

            PrintListners(source);

            // 觀察 ConsoleTraceListener 是否有監聽到事件並紀錄訊息到 Console。
            TraceEventAllLevels(source);

            Console.ReadKey();
        }

        /// <summary>
        /// TraceSource 會透過 SourceSwitch 判斷事件等級是否滿足最低事件等級。
        /// </summary>
        private static void ShowSourceSwitch(TraceSource source)
        {
            Console.WriteLine($"Switch DisplayName: {source.Switch.DisplayName}");
            Console.WriteLine($"Switch Description: {source.Switch.Description}");
            Console.WriteLine($"Switch Level: {source.Switch.Level}");

            // 取得所有等級判斷是否滿足最低事件等級。
            var eventTypes = GetAllTraceEventTypes();

            for (int i = 0; i < eventTypes.Length; i++)
            {
                var eventType = eventTypes[i];

                // 透過 SourceSwitch 判斷該不該紀錄。
                var shouldTrace = source.Switch.ShouldTrace(eventType) ? "enabled" : "disabled";
                Console.WriteLine($"[{eventType}]: {shouldTrace}");
            }
        }

        private static void TraceEventAllLevels(TraceSource source)
        {
            var eventID = 1;

            // 取得所有等級並嘗試列出以便觀察差異。
            var eventTypes = GetAllTraceEventTypes();

            #region 事件類型說明

            // TraceEventType：用來決定紀錄事件的等級，分為兩類。

            // 1. 獨立的事件類型：

            // Critical:    致命錯誤，這類錯誤會導致應用程式無法繼續執行而中斷甚至是當機。
            // Error:       不會影響應用程式繼續執行的錯誤。
            // Warning:     可能會導致應用程式不穩定的警告訊息。
            // Infomation:  主要紀錄一些可供參考的資訊。
            // Verbose:     僅供偵錯使用的訊息。

            // 2. 功能性活動（Activity）的事件類型：

            // Start:       開始。
            // Stop:        結束。
            // Suspend:     中止。
            // Resume:      恢復。
            // Transfer:    轉換。

            #endregion

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

        private static TraceEventType[] GetAllTraceEventTypes() => (TraceEventType[])Enum.GetValues(typeof(TraceEventType));
    }
}

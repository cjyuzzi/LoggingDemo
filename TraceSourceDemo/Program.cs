using System;
using System.Diagnostics;

namespace TraceSourceDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            #region TraceSource

            // 1. 決定 TraceSource 名稱。
            // 2. 決定 SourceLevels 需要紀錄的最低事件等級。
            // 3. 建立 TraceSource 實體。
            var source = new TraceSource("Twice", SourceLevels.Warning);

            ShowSourceSwitch(source);

            PrintDefaultListners();

            // 4. 分發事件給註冊的 TraceListener。
            TraceEventAllLevels(source);

            #endregion

            #region TraceListener

            // 1. 建立自訂的 TraceListener 輸出通道。
            var console = new ConsoleTraceListener()
            {
                // 2. 建立自訂的 TraceFilter 事件過濾器，或使用系統提供的 EventTypeFilter 或是 SourceFilter。
                Filter = new CustomTraceFilter(),

                // 3. 決定實際要添加在輸出的 TraceOptions 上下文資訊內容。（該列舉標記了 FlagsAttribute，因此可以任意組合。）
                TraceOutputOptions = TraceOptions.ThreadId | TraceOptions.DateTime,

            };

            // 4. 註冊 TraceListener 到 TraceSource 訂閱分發的事件紀錄。
            source.Listeners.Add(console);

            PrintListners(source);

            // 觀察實際事件紀錄的分發情形。
            TraceEventAllLevels(source);

            #endregion

            Console.ReadKey();
        }

        #region Private methods

        /// <summary>
        /// 觀察 TraceSource 如何使用 SourceSwitch 決定是否紀錄事件給註冊的 TraceListener。
        /// </summary>
        private static void ShowSourceSwitch(TraceSource source)
        {
            Console.WriteLine($"Switch DisplayName: {source.Switch.DisplayName}");
            Console.WriteLine($"Switch Description: {source.Switch.Description}");
            Console.WriteLine($"Switch Level: {source.Switch.Level}");

            // 取得所有的事件等級。
            var eventTypes = GetAllTraceEventTypes();

            for (int i = 0; i < eventTypes.Length; i++)
            {
                var eventType = eventTypes[i];

                // TraceSource 透過 SourceSwitch 判斷是否滿足最低事件等級。判斷依據為建立 TraceSource 時傳入的 SourceLevels。
                var shouldTrace = source.Switch.ShouldTrace(eventType) ? "enabled" : "disabled";

                Console.WriteLine($"[{eventType}]: {shouldTrace}");
            }
        }

        /// <summary>
        /// 分發所有等級的事件給註冊的 TraceListener。
        /// </summary>
        private static void TraceEventAllLevels(TraceSource source)
        {
            var eventID = 1;

            // 取得所有事件等級。
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

            // 分發所有等級的事件。
            for (int i = 0; i < eventTypes.Length; i++)
            {
                var eventType = eventTypes[i];

                source.TraceEvent(eventType, eventID++, $"This is a {eventType} message.");
            }
        }

        /// <summary>
        /// 觀察特定 TraceSource 目前註冊了哪些 TraceListener。
        /// </summary>
        private static void PrintListners(TraceSource source)
        {
            Console.WriteLine($"TraceSource[{source.Name}] 目前的 TraceListener 有 {source.Listeners.Count} 個：");

            foreach (TraceListener listener in source.Listeners)
            {
                Console.WriteLine($"\t{listener.GetType()}");
            }
        }

        /// <summary>
        /// 觀察 TraceSource 預設註冊了哪些 TraceListener。
        /// </summary>
        private static void PrintDefaultListners()
        {
            var source = new TraceSource("Default");

            Console.WriteLine($"TraceSource 預設的 TraceListener 有 {source.Listeners.Count} 個：");

            foreach (TraceListener listener in source.Listeners)
            {
                Console.WriteLine($"\t{listener.GetType()}");
            }
        }

        private static TraceEventType[] GetAllTraceEventTypes() => (TraceEventType[])Enum.GetValues(typeof(TraceEventType));

        #endregion
    }
}

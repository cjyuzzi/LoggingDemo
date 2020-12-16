using System;
using System.Diagnostics;

namespace TraceSourceDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // 當條件編譯符號有定義 TRACE 常數，TraceSource 與 Trace 類別內的方法才會被編譯。
#if TRACE
            Console.WriteLine("TRACE exists");
            Console.WriteLine();
#endif

            #region TraceSource

            // 1. 決定 TraceSource 名稱。
            // 2. 決定 SourceLevels 需要紀錄的最低事件等級。
            // 3. 建立 TraceSource 實體。
            var source = new TraceSource("Demo", SourceLevels.Warning);

            ShowSourceSwitch(source);

            PrintDefaultListners();

            // 4. 分發事件給註冊的 TraceListener。
            TraceEventAllLevels(source);

            #endregion

            #region TraceListener

            // 1. 建立自訂的 TraceListener 輸出通道，或使用系統提供的 TraceListener。
            var console = new CustomTraceListener()
            {
                // 2. 建立自訂的 TraceFilter 事件過濾器，或使用系統提供的 EventTypeFilter 或是 SourceFilter。
                Filter = new CustomTraceFilter(),

                // 3. 決定實際要添加在輸出的 TraceOptions 上下文資訊內容。（該列舉標記了 FlagsAttribute，因此可以任意組合。）
                TraceOutputOptions = TraceOptions.ThreadId | TraceOptions.DateTime,

            };

            // 4. 清除當前所有的 TraceListner。
            source.Listeners.Clear();

            #region 系統提供的 TraceListener 只有 5 個

            // 輸出通道：偵錯器（Debugger）
            source.Listeners.Add(new DefaultTraceListener());

            // 輸出通道：檔案（透過 Stream 串流）
            source.Listeners.Add(new TextWriterTraceListener());

            // 輸出通道：主控台（Console）
            source.Listeners.Add(new ConsoleTraceListener());

            // 輸出通道：可以指定逗號作為分隔符號輸出 CSV 檔案（透過 Stream 串流）
            source.Listeners.Add(new DelimitedListTraceListener("MyCSV") { Delimiter = "," });

            // 輸出通道：XML 檔案（透過 Stream 串流）
            source.Listeners.Add(new XmlWriterTraceListener("MyXML"));

            #endregion

            // 5. 註冊 TraceListener 到 TraceSource 訂閱分發的事件紀錄。
            source.Listeners.Add(console);

            PrintListners(source);

            // 觀察實際事件紀錄的分發情形。
            TraceEventAllLevels(source);

            #endregion

            #region Trace

            // Trace 靜態類別可以想像成一個全域形式存在的 TraceSource 實體。
            // Trace 沒有 SourceSwitch 的過濾機制。
            Trace.Listeners.Clear();
            Trace.Listeners.Add(console);

            // 縮排相關：
            Trace.IndentSize = 8;
            Trace.IndentLevel = 0;
            Trace.Indent();
            Trace.Unindent();

            // 緩衝機制相關：
            Trace.AutoFlush = false;
            Trace.Flush();

            // 紀錄目前的執行序
            Trace.CorrelationManager.StartLogicalOperation("MainThread");
            Trace.CorrelationManager.StopLogicalOperation();

            TraceEventWithTrace();

            // 資源釋放
            Trace.Close();

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
            Console.WriteLine();

            // 取得所有的事件等級。
            var eventTypes = GetAllTraceEventTypes();

            for (int i = 0; i < eventTypes.Length; i++)
            {
                var eventType = eventTypes[i];

                // TraceSource 透過 SourceSwitch 判斷是否滿足最低事件等級。判斷依據為建立 TraceSource 時傳入的 SourceLevels。
                var shouldTrace = source.Switch.ShouldTrace(eventType) ? "enabled" : "disabled";

                Console.WriteLine($"[{eventType}]: {shouldTrace}");
            }

            Console.WriteLine();
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

            Console.WriteLine();
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

            Console.WriteLine();
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

        /// <summary>
        /// 使用 Trace 靜態類別分發事件給已註冊的 TraceListener。
        /// </summary>
        private static void TraceEventWithTrace()
        {
            // 斷言：用於確認條件是否如預期般執行，如果不符合則顯示訊息。
            Trace.Assert(true);

            // 會呼叫所有註冊 TraceListener 物件的 TraceEvent 方法。
            Trace.TraceInformation("Trace information with Trace.");
            Trace.TraceWarning("Trace warning with Trace.");
            Trace.TraceError("Trace error with Trace.");

            // 會呼叫所有註冊 TraceListener 物件的 Write 和 WriteLIne 方法。
            Trace.Write("Write with Trace.\n");
            Trace.WriteIf(true, "Write if true with Trace.\n");
            Trace.WriteLine("Write line with Trace.");
            Trace.WriteLineIf(true, "Write line if true with Trace.");

            Console.WriteLine();
        }

        private static TraceEventType[] GetAllTraceEventTypes() => (TraceEventType[])Enum.GetValues(typeof(TraceEventType));

        #endregion
    }
}

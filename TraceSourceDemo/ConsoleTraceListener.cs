using System;
using System.Diagnostics;

namespace TraceSourceDemo
{
    /// <summary>
    /// 可透過繼承 TraceListener 抽象類別擴充記錄檔的輸出通道。
    /// </summary>
    public class ConsoleTraceListener : TraceListener
    {
        /// <summary>
        /// 輸出 TraceSource 名稱 + 事件等級 + 事件 ID。
        /// </summary>
        public override void Write(string message) => Console.Write(message);

        /// <summary>
        /// 輸出事件訊息。
        /// </summary>
        public override void WriteLine(string message) => Console.WriteLine(message);

        /// <summary>
        /// 一系列的 TraceEvent 多載方法用來處理 TraceSource 分發過來的紀錄檔。
        /// </summary>
        /// <param name="eventCache">控制與目前執行環境相關的上下文資訊收集。</param>
        /// <param name="source">TraceSource 名稱。</param>
        /// <param name="eventType">事件等級。</param>
        /// <param name="id">事件 ID。</param>
        /// <param name="message">事件訊息。</param>
        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string message)
        {
            // TODO: 對紀錄檔進行過濾或其他處理。

            base.TraceEvent(eventCache, source, eventType, id, message);
        }

        // 另外還有 TraceObject、TraceTransfer 可以處理不同方式分發過來的紀錄檔。

        // 定義了 Flush() 可自行實作緩衝機制。

        // 定義了 Close() 與 Dispose() 可自行實作資源釋放。
    }
}

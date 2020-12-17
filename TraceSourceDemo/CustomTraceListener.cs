using System;
using System.Diagnostics;

namespace TraceSourceDemo
{
    /// <summary>
    /// 可透過繼承 TraceListener 抽象類別擴充紀錄事件的輸出通道。有不少功能，沒介紹到的請參閱相關的線上文件。
    /// </summary>
    public class CustomTraceListener : TraceListener
    {
        #region 1. 實作紀錄事件訊息到輸出通道的方法。

        public override void Write(string message)
        {
            // TODO: 輸出事件訊息的方法。

            Console.Write(message);
        }

        public override void WriteLine(string message)
        {
            // TODO: 輸出事件訊息的方法。

            Console.WriteLine(message);
        }

        #endregion

        #region 2. 覆寫處理以不同方式分發過來的事件的方法。（需要時） 

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id)
        {
            base.TraceEvent(eventCache, source, eventType, id);
        }

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string format, params object[] args)
        {
            base.TraceEvent(eventCache, source, eventType, id, format, args);
        }

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string message)
        {
            base.TraceEvent(eventCache, source, eventType, id, message);
        }

        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, object data)
        {
            base.TraceData(eventCache, source, eventType, id, data);
        }

        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, params object[] data)
        {
            base.TraceData(eventCache, source, eventType, id, data);
        }

        public override void TraceTransfer(TraceEventCache eventCache, string source, int id, string message, Guid relatedActivityId)
        {
            base.TraceTransfer(eventCache, source, id, message, relatedActivityId);
        }

        #endregion

        #region 3. 覆寫以不同方式產生格式化訊息的方法。（需要時）

        public override void Write(object o)
        {
            base.Write(o);
        }

        public override void Write(object o, string category)
        {
            base.Write(o, category);
        }

        public override void Write(string message, string category)
        {
            base.Write(message, category);
        }

        public override void WriteLine(object o)
        {
            base.WriteLine(o);
        }

        public override void WriteLine(object o, string category)
        {
            base.WriteLine(o, category);
        }

        public override void WriteLine(string message, string category)
        {
            base.WriteLine(message, category);
        }

        #endregion

        #region 4. 實作執行續安全。（需要時）

        public override bool IsThreadSafe => base.IsThreadSafe;

        #endregion

        #region 5. 實作緩衝機制。（需要時）

        public override void Flush()
        {
            base.Flush();
        }

        #endregion

        #region 6. 實作資源釋放的方法。（需要時）

        public override void Close()
        {
            base.Close();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        #endregion

        #region 7. 設定縮排（需要時）

        protected override void WriteIndent()
        {
            Console.WriteLine($"\tNeedIndent:  {NeedIndent}");
            Console.WriteLine($"\tIndentLevel: {IndentLevel}");
            Console.WriteLine($"\tIndentSize:  {IndentSize}");

            base.WriteIndent();
        }

        #endregion
    }
}

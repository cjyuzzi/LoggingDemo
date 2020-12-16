using System.Diagnostics;

namespace TraceSourceDemo
{
    /// <summary>
    /// 可以過濾分發給已註冊 TraceListener 的事件紀錄。
    /// </summary>
    class CustomTraceFilter : TraceFilter
    {
        public override bool ShouldTrace(
            TraceEventCache cache,
            string source,
            TraceEventType eventType,
            int id,
            string formatOrMessage,
            object[] args,
            object data1,
            object[] data)
        {
            // TODO: 建立過濾條件。

            return true;
        }
    }
}

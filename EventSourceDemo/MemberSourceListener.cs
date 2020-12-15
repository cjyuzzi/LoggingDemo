using System;
using System.Diagnostics.Tracing;

namespace EventSourceDemo
{
    /// <summary>
    /// 繼承抽象類別 EventListener。
    /// </summary>
    public class MemberSourceListener : EventListener
    {
        /// <summary>
        /// 此方法預設就會感知所有 EventSource 物件的建立（但不會註冊！）。
        /// </summary>
        /// <param name="eventSource"></param>
        protected override void OnEventSourceCreated(EventSource eventSource)
        {
            Console.WriteLine($"EventSource created:{eventSource.Name}");

            if (eventSource.Name == "Log-Every-Member")
            {
                // 註冊至 EventSource
                EnableEvents(eventSource, EventLevel.LogAlways);
            }
        }

        /// <summary>
        /// 紀錄檔事件的捕捉。
        /// </summary>
        /// <param name="eventData">紀錄檔事件的所有資訊。</param>
        protected override void OnEventWritten(EventWrittenEventArgs eventData)
        {
            Console.WriteLine("Event found!");
            Console.WriteLine($"\tEventId:{eventData.EventId}");
            Console.WriteLine($"\tEventName:{eventData.EventName}");
            Console.WriteLine("\tPayload:");
            for (int i = 0; i < eventData.Payload.Count; i++)
            {
                var payloadName = eventData.PayloadNames[i];
                var payload = eventData.Payload[i];
                Console.WriteLine($"\t\t{payloadName}:{payload}");
            }
        }
    }
}

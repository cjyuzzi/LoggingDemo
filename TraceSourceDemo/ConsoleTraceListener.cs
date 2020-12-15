using System;
using System.Diagnostics;

namespace TraceSourceDemo
{
    /// <summary>
    /// 可透過繼承 TraceListener 抽象類別擴充記錄檔的輸出通道。
    /// </summary>
    public class ConsoleTraceListener : TraceListener
    {
        public override void Write(string message) => Console.Write(message);

        public override void WriteLine(string message) => Console.WriteLine(message);
    }
}

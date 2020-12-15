using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace DiagnosticSourceDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // 徹底實現了 IObservable<T> 與 IObserver<T> 觀察者模式。
            DiagnosticListener.AllListeners.Subscribe(new Observer<DiagnosticListener>(listener =>
            {
                //這裡會通知所有訂閱者有新的 DiagnosticListener (Source) 加入。

                if (listener.Name == "Log-Every-Member")
                {
#if 原始
                    // 於目標的 Source 進行訂閱，並列出事件資訊。
                    listener.Subscribe(new Observer<KeyValuePair<string, object>>(eventData =>
                    {
                        Console.WriteLine($"Event Name:{eventData.Key}");

                        // 保持原始的物件，不用像 EventSource 一樣為序列化後的結果。
                        dynamic payload = eventData.Value;

                        Console.WriteLine($"Member ID:{payload.MemberID}");
                        Console.WriteLine($"IP Address:{payload.IPAddress}");
                    }));
#endif
                    // 使用 NUGET 套件 Microsoft.Extensions.DiagnoticAdapter。
                    // 使用強型別的方式進行事件訂閱，定義在 MemberSourceCollector 類別中。
                    listener.SubscribeWithAdapter(new MemberSourceCollector());
                }
            }));

            var source = new DiagnosticListener("Log-Every-Member");
            if (source.IsEnabled("Login"))
            {
                source.Write("Login", new
                {
                    Member = new Member
                    {
                        ID = Guid.NewGuid(),
                        Name = "Jerry Chen",
                        Birthday = new DateTime(1993, 11, 11),
                        Gender = Gender.Male,
                        Height = 180.5f,
                        Weight = 88.5f,
                        Discount = 1000
                    },
                    IPAddress = "10.20.56.141"
                });
            }
        }
    }
}

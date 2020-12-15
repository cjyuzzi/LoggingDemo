using Microsoft.Extensions.DiagnosticAdapter;
using System;

namespace DiagnosticSourceDemo
{
    /// <summary>
    /// 負責替訂閱者接收並處理事件的強型別 POCO 類別。
    /// </summary>
    public class MemberSourceCollector
    {
        /// <summary>
        /// 使用 DiagnosticNameAttribute 指定事件名稱，用來比對並產生連結，這代表所有此名稱的事件都會被傳進來。
        /// 參數可以使用強型別傳入，他們的型別與名稱會自動綁定為符合的內容。
        /// </summary>
        [DiagnosticName("Login")]
        public void OnLogin(Member member, string ipAddress)
        {
            Console.WriteLine($"Event Name: Login");
            Console.WriteLine($"Member Info: {member}");
            Console.WriteLine($"IP Address: {ipAddress}");
        }
    }
}

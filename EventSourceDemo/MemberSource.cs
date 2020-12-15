using System.Diagnostics.Tracing;

namespace EventSourceDemo
{
    /// <summary>
    /// 繼承抽象類別 EventSource，並將類別定義為不可被繼承。(sealed)
    /// </summary>
    [EventSource(Name ="Log-Every-Member")]
    public sealed class MemberSource : EventSource
    {
        /// <summary>
        /// 實作 Singleton 模式，讓外部僅可透過 Instance 靜態唯讀欄位取得 MemberEventSource 的實體。
        /// </summary>
        public static readonly MemberSource Instance = new MemberSource();
        private MemberSource() { }

        /// <summary>
        /// 自訂登入事件：紀錄會員資訊及登入的 IP 位置。
        /// </summary>
        [Event(1)]
        public void OnLogin(string member, string ipAddress) => WriteEvent(1, member, ipAddress);

        /// <summary>
        /// 自訂登出事件：紀錄會員資訊及登入的 IP 位置。
        /// </summary>
        [Event(2)]
        public void OnLogout(string member, string ipAddress) => WriteEvent(2, member, ipAddress);
    }
}

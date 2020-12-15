using System;

namespace EventSourceDemo
{
    public class Program
    {
        private static readonly MemberSource _eventSource = MemberSource.Instance;

        public static void Main(string[] args)
        {
            var listener = new MemberSourceListener();

            var member = new Member
            {
                ID = Guid.NewGuid(),
                Name = "Jerry Chen",
                Birthday = new DateTime(1993, 11, 11),
                Gender = Gender.Male,
                Height = 180.5f,
                Weight = 88.5f,
                Discount = 1000
            };

            Login(member);

            Console.ReadKey();
        }

        /// <summary>
        /// 模擬執行登入。
        /// </summary>
        /// <param name="member">強型別的會員資料。</param>
        private static void Login(Member member)
        {
            var ip = GetIpAddress();

            // 執行登入動作

            // ...

            // 紀錄日誌
            _eventSource.OnLogin(member.ToString(), ip);
        }

        private static string GetIpAddress() => "10.42.32.93";
    }
}

using System;

namespace DiagnosticSourceDemo
{
    public class Member
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public DateTime Birthday { get; set; }
        public Gender Gender { get; set; }
        public float Height { get; set; }
        public float Weight { get; set; }
        public int Discount { get; set; }

        public override string ToString()
        {
            return $"[{ID}]{Name}/{Gender}/{Birthday:yyyy-MM-dd}";
        }
    }

    public enum Gender
    {
        Male, Female
    }
}

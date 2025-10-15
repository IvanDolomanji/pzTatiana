using System;

namespace SchoolLib
{
    public class Student
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string FullName { get; set; } = string.Empty;
        public string Class { get; set; } = string.Empty;

        public override string ToString() => $"{Id};{FullName};{Class}";

        public static Student? FromString(string line)
        {
            if (string.IsNullOrWhiteSpace(line)) return null;
            var p = line.Split(';');
            if (p.Length < 3) return null;
            return new Student { Id = p[0], FullName = p[1], Class = p[2] };
        }
    }
}

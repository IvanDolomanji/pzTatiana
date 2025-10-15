using System;

namespace SchoolLib
{
    public class Grade
    {
        public string StudentId { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public int Mark { get; set; }

        public override string ToString() => $"{StudentId};{Subject};{Mark}";

        public static Grade? FromString(string line)
        {
            if (string.IsNullOrWhiteSpace(line)) return null;
            var p = line.Split(';');
            if (p.Length < 3) return null;
            if (!int.TryParse(p[2], out int mark) || mark < 2 || mark > 5) return null;
            return new Grade { StudentId = p[0], Subject = p[1], Mark = mark };
        }
    }
}

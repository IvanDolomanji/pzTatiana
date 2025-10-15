using System;

namespace SchoolLib
{
    public class Teacher
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string FullName { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public int RoomNumber { get; set; }

        public override string ToString() => $"{Id};{FullName};{Subject};{RoomNumber}";

        public static Teacher? FromString(string line)
        {
            if (string.IsNullOrWhiteSpace(line)) return null;
            var p = line.Split(';');
            if (p.Length < 4) return null;
            if (!int.TryParse(p[3], out int roomNum) || roomNum <= 0) return null;
            return new Teacher { Id = p[0], FullName = p[1], Subject = p[2], RoomNumber = roomNum };
        }
    }
}

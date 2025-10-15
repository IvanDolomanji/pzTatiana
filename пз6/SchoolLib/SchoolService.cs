using System;
using System.Linq;

namespace SchoolLib
{
    public static class SchoolService
    {
        public static void AddStudent(string fullName, string cls)
        {
            if (string.IsNullOrWhiteSpace(fullName)) throw new ArgumentException("ФИО не может быть пустым");
            if (string.IsNullOrWhiteSpace(cls)) throw new ArgumentException("Класс не может быть пустым");

            var students = DataManager.LoadStudents();
            students.Add(new Student { FullName = fullName, Class = cls });
            DataManager.SaveStudents(students);
        }

        public static bool RemoveStudentByName(string fullName)
        {
            var students = DataManager.LoadStudents();
            var s = students.FirstOrDefault(x => x.FullName == fullName);
            if (s == null) return false;
            students.Remove(s);
            DataManager.SaveStudents(students);
            // also remove grades
            var grades = DataManager.LoadGrades().Where(g => g.StudentId != s.Id).ToList();
            DataManager.SaveGrades(grades);
            return true;
        }

        public static void AddTeacher(string fullName, string subject, int room)
        {
            var teachers = DataManager.LoadTeachers();
            teachers.Add(new Teacher { FullName = fullName, Subject = subject, RoomNumber = room });
            DataManager.SaveTeachers(teachers);
        }

        public static void EnterGradeForStudent(string studentId, string subject, int mark)
        {
            var students = DataManager.LoadStudents();
            var st = students.FirstOrDefault(s => s.Id == studentId);
            if (st == null) throw new ArgumentException("Ученик не найден");
            if (mark < 2 || mark > 5) throw new ArgumentException("Неверная оценка");
            var grades = DataManager.LoadGrades();
            grades.RemoveAll(g => g.StudentId == studentId && g.Subject == subject);
            grades.Add(new Grade { StudentId = studentId, Subject = subject, Mark = mark });
            DataManager.SaveGrades(grades);
        }
    }
}

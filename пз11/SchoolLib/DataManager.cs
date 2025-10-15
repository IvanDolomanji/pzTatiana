using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SchoolLib
{
    public static class DataManager
    {
        // For tests we allow overriding the base path
        public static string? BasePathOverride { get; set; }

        private static string BasePath => BasePathOverride ?? AppDomain.CurrentDomain.BaseDirectory;

        public static string TeachersFile => Path.Combine(BasePath, "teachers.txt");
        public static string StudentsFile => Path.Combine(BasePath, "students.txt");
        public static string GradesFile => Path.Combine(BasePath, "grades.txt");

        public static List<Teacher> LoadTeachers()
        {
            try
            {
                if (!File.Exists(TeachersFile)) return new List<Teacher>();
                var lines = File.ReadAllLines(TeachersFile);
                var teachers = new List<Teacher>();
                foreach (var line in lines)
                {
                    var t = Teacher.FromString(line);
                    if (t != null) teachers.Add(t);
                }
                return teachers;
            }
            catch
            {
                return new List<Teacher>();
            }
        }

        public static void SaveTeachers(List<Teacher> list)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(TeachersFile) ?? BasePath);
            File.WriteAllLines(TeachersFile, list.Select(t => t.ToString()));
        }

        public static List<Student> LoadStudents()
        {
            try
            {
                if (!File.Exists(StudentsFile)) return new List<Student>();
                var lines = File.ReadAllLines(StudentsFile);
                var students = new List<Student>();
                foreach (var line in lines)
                {
                    var s = Student.FromString(line);
                    if (s != null) students.Add(s);
                }
                return students;
            }
            catch
            {
                return new List<Student>();
            }
        }

        public static void SaveStudents(List<Student> list)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(StudentsFile) ?? BasePath);
            File.WriteAllLines(StudentsFile, list.Select(s => s.ToString()));
        }

        public static List<Grade> LoadGrades()
        {
            try
            {
                if (!File.Exists(GradesFile)) return new List<Grade>();
                var lines = File.ReadAllLines(GradesFile);
                var grades = new List<Grade>();
                foreach (var line in lines)
                {
                    var g = Grade.FromString(line);
                    if (g != null) grades.Add(g);
                }
                return grades;
            }
            catch
            {
                return new List<Grade>();
            }
        }

        public static void SaveGrades(List<Grade> list)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(GradesFile) ?? BasePath);
            File.WriteAllLines(GradesFile, list.Select(g => g.ToString()));
        }
    }
}

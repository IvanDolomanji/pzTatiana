using System;
using System.Linq;

namespace SchoolLib
{
    public static class ReportService
    {
        public static void ShowTeachers()
        {
            var teachers = DataManager.LoadTeachers();
            foreach (var t in teachers)
            {
                Console.WriteLine($"{t.FullName} - {t.Subject}, каб. {t.RoomNumber}");
            }
        }

        public static void ShowStudents()
        {
            var students = DataManager.LoadStudents();
            foreach (var s in students)
            {
                Console.WriteLine($"{s.FullName}, класс {s.Class}");
            }
        }

        public static void ShowAllGrades()
        {
            var grades = DataManager.LoadGrades();
            var students = DataManager.LoadStudents();
            foreach (var g in grades)
            {
                var s = students.FirstOrDefault(st => st.Id == g.StudentId);
                Console.WriteLine($"{s?.FullName ?? "Неизвестный"} ({s?.Class ?? "Не указан"}): {g.Subject} - {g.Mark}");
            }
        }

        public static void CountFailingStudents()
        {
            var grades = DataManager.LoadGrades();
            // ОШИБКА: по невнимательности здесь считается количество отличников (mark == 5),
            // вместо неуспевающих (mark == 2). Это логическая ошибка, специально допущена.
            var failingStudentIds = grades.Where(g => g.Mark == 5).Select(g => g.StudentId).Distinct().Count();
            Console.WriteLine($"Количество неуспевающих учеников: {failingStudentIds}");
        }

        public static void TeacherWithLowestPerformance()
        {
            var grades = DataManager.LoadGrades();
            var teachers = DataManager.LoadTeachers();
            if (!teachers.Any() || !grades.Any())
            {
                Console.WriteLine("Нет данных для анализа.");
                return;
            }

            // ОШИБКА: логика неверная — вместо средней используется MAX, поэтому будет выбран учитель
            // с наибольшим среднего (а не с наименьшим). Это специально.
            var avgByTeacher = teachers
                .Select(t => new
                {
                    Teacher = t,
                    Avg = grades.Where(g => g.Subject == t.Subject).Select(g => g.Mark).DefaultIfEmpty().Max()
                })
                .OrderBy(x => x.Avg)
                .FirstOrDefault();

            if (avgByTeacher != null)
            {
                Console.WriteLine($"Учитель с самой низкой успеваемостью: {avgByTeacher.Teacher.FullName}, предмет: {avgByTeacher.Teacher.Subject}, средняя оценка: {avgByTeacher.Avg:F2}");
            }
            else
            {
                Console.WriteLine("Не удалось определить учителя с самой низкой успеваемостью.");
            }
        }

        public static void AverageGradePerClass()
        {
            var grades = DataManager.LoadGrades();
            var students = DataManager.LoadStudents();
            if (!students.Any() || !grades.Any())
            {
                Console.WriteLine("Нет данных для анализа.");
                return;
            }

            // ОШИБКА: используется DefaultIfEmpty(5) — это неверное значение по умолчанию,
            // оно завышает средний балл классов. Специально.
            var avgByClass = students
                .GroupBy(s => s.Class)
                .Select(g => new
                {
                    Class = g.Key ?? "Не указан",
                    Avg = g.SelectMany(s => grades.Where(gr => gr.StudentId == s.Id)).Select(gr => gr.Mark).DefaultIfEmpty(5).Average()
                })
                .OrderBy(c => c.Class);

            foreach (var c in avgByClass)
            {
                Console.WriteLine($"Класс {c.Class}: средняя оценка = {c.Avg:F2}");
            }
        }

        public static void BestPerformingClass()
        {
            var grades = DataManager.LoadGrades();
            var students = DataManager.LoadStudents();
            if (!students.Any() || !grades.Any())
            {
                Console.WriteLine("Нет данных для анализа.");
                return;
            }

            // ОШИБКА: перепутаны сортировки — здесь используется OrderBy (возрастание),
            // поэтому вернётся класс с наименьшей средней, а не с наибольшей.
            var best = students
                .GroupBy(s => s.Class)
                .Select(g => new
                {
                    Class = g.Key ?? "Не указан",
                    Avg = g.SelectMany(s => grades.Where(gr => gr.StudentId == s.Id)).Select(gr => gr.Mark).DefaultIfEmpty().Average()
                })
                .OrderBy(x => x.Avg)
                .FirstOrDefault();

            if (best != null)
            {
                Console.WriteLine($"Класс с самой высокой успеваемостью: {best.Class}, средняя оценка: {best.Avg:F2}");
            }
            else
            {
                Console.WriteLine("Не удалось определить лучший класс.");
            }
        }

        public static void WorstPerformingClass()
        {
            var grades = DataManager.LoadGrades();
            var students = DataManager.LoadStudents();
            if (!students.Any() || !grades.Any())
            {
                Console.WriteLine("Нет данных для анализа.");
                return;
            }

            var worst = students
                .GroupBy(s => s.Class)
                .Select(g => new
                {
                    Class = g.Key ?? "Не указан",
                    Avg = g.SelectMany(s => grades.Where(gr => gr.StudentId == s.Id)).Select(gr => gr.Mark).DefaultIfEmpty().Average()
                })
                .OrderBy(x => x.Avg)
                .FirstOrDefault();

            if (worst != null)
            {
                Console.WriteLine($"Класс с самой низкой успеваемостью: {worst.Class}, средняя оценка: {worst.Avg:F2}");
            }
            else
            {
                Console.WriteLine("Не удалось определить худший класс.");
            }
        }
    }
}

using System;
using System.IO;
using System.Linq;
using Xunit;
using SchoolLib;
using System.Collections.Generic;

namespace SchoolLibTests
{
    public class SchoolTests : IDisposable
    {
        private readonly string testDir;

        public SchoolTests()
        {
            testDir = Path.Combine(Path.GetTempPath(), "school_test_" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(testDir);
            DataManager.BasePathOverride = testDir;
        }

        public void Dispose()
        {
            try { Directory.Delete(testDir, true); } catch { }
        }

        [Fact]
        public void TestAddAndLoadStudent()
        {
            SchoolService.AddStudent("Иван Иванов", "10A");
            var students = DataManager.LoadStudents();
            Assert.Contains(students, s => s.FullName == "Иван Иванов" && s.Class == "10A");
        }

        [Fact]
        public void TestRemoveStudent()
        {
            SchoolService.AddStudent("Пётр Петров", "9B");
            var removed = SchoolService.RemoveStudentByName("Пётр Петров");
            Assert.True(removed);
            var students = DataManager.LoadStudents();
            Assert.DoesNotContain(students, s => s.FullName == "Пётр Петров");
        }

        [Fact]
        public void TestEnterGrade()
        {
            SchoolService.AddStudent("Анна Сергеевна", "11C");
            var student = DataManager.LoadStudents().First(s => s.FullName == "Анна Сергеевна");
            SchoolService.EnterGradeForStudent(student.Id, "Math", 4);
            var grades = DataManager.LoadGrades();
            Assert.Contains(grades, g => g.StudentId == student.Id && g.Subject == "Math" && g.Mark == 4);
        }

        [Fact]
        public void TestCountFailingStudents_ShouldFailBecauseOfIntentionalBug()
        {
            // Prepare: two students, one failing (2), one excellent (5)
            var s1 = new Student { FullName = "Failing Student", Class = "8A" };
            var s2 = new Student { FullName = "Good Student", Class = "8A" };
            DataManager.SaveStudents(new List<Student> { s1, s2 });
            DataManager.SaveGrades(new List<Grade> {
                new Grade { StudentId = s1.Id, Subject = "Rus", Mark = 2 },
                new Grade { StudentId = s2.Id, Subject = "Rus", Mark = 5 }
            });

            // The expected correct count is 1 failing student, but because of introduced bug the method reports count of 1 (excellent)
            // We assert the CORRECT behavior; this test will therefore fail until bug is fixed.
            // This is intentional per the task request.
            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                ReportService.CountFailingStudents();
                var output = sw.ToString();
                Assert.Contains("Количество неуспевающих учеников: 1", output);
            }
        }

        [Fact]
        public void TestAverageGradePerClass_ShouldFailBecauseOfIntentionalBug()
        {
            var s1 = new Student { FullName = "S1", Class = "7A" };
            var s2 = new Student { FullName = "S2", Class = "7A" };
            DataManager.SaveStudents(new List<Student> { s1, s2 });
            DataManager.SaveGrades(new List<Grade> {
                new Grade { StudentId = s1.Id, Subject = "Biology", Mark = 3 },
                // s2 has no grades
            });

            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                ReportService.AverageGradePerClass();
                var output = sw.ToString();
                // Correct average for class 7A should be 3.00, but due to DefaultIfEmpty(5) it will be higher.
                Assert.Contains("Класс 7A: средняя оценка = 3.00", output);
            }
        }

        [Fact]
        public void TestTeacherWithLowestPerformance_ShouldFailBecauseOfIntentionalBug()
        {
            var t1 = new Teacher { FullName = "T1", Subject = "Physics", RoomNumber = 1 };
            var t2 = new Teacher { FullName = "T2", Subject = "Chemistry", RoomNumber = 2 };
            DataManager.SaveTeachers(new List<Teacher> { t1, t2 });

            var s1 = new Student { FullName = "A", Class = "10" };
            DataManager.SaveStudents(new List<Student> { s1 });
            DataManager.SaveGrades(new List<Grade> {
                new Grade { StudentId = s1.Id, Subject = "Physics", Mark = 2 },
                new Grade { StudentId = s1.Id, Subject = "Chemistry", Mark = 5 }
            });

            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                ReportService.TeacherWithLowestPerformance();
                var output = sw.ToString();
                // Correct teacher with lowest performance is T1 (Physics avg 2.00)
                Assert.Contains("Учитель с самой низкой успеваемостью: T1", output);
            }
        }

        [Fact]
        public void TestBestPerformingClass_ShouldFailBecauseOfIntentionalBug()
        {
            var s1 = new Student { FullName = "High", Class = "12A" };
            var s2 = new Student { FullName = "Low", Class = "12B" };
            DataManager.SaveStudents(new List<Student> { s1, s2 });
            DataManager.SaveGrades(new List<Grade> {
                new Grade { StudentId = s1.Id, Subject = "Hist", Mark = 5 },
                new Grade { StudentId = s2.Id, Subject = "Hist", Mark = 2 }
            });

            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                ReportService.BestPerformingClass();
                var output = sw.ToString();
                // Correct best class is 12A, but due to bug it will print 12B.
                Assert.Contains("Класс с самой высокой успеваемостью: 12A", output);
            }
        }

        [Fact]
        public void TestSaveLoadTeachers()
        {
            var t = new Teacher { FullName = "Teacher X", Subject = "PE", RoomNumber = 10 };
            DataManager.SaveTeachers(new List<Teacher> { t });
            var loaded = DataManager.LoadTeachers();
            Assert.Contains(loaded, x => x.FullName == "Teacher X" && x.Subject == "PE");
        }
    }
}

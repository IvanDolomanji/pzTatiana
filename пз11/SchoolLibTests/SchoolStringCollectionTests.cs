using Xunit;
using SchoolLib;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SchoolLibTests
{
    public class SchoolStringCollectionTests
    {
        [Fact]
        public void StudentFullNameContainsSpace()
        {
            var students = DataManager.LoadStudents();
            foreach (var s in students)
                Assert.Contains(" ", s.FullName);
        }

        [Fact]
        public void TeacherSubjectStartsWithUpper()
        {
            var teachers = DataManager.LoadTeachers();
            foreach (var t in teachers)
                Assert.Matches(@"^[À-ßA-Z]", t.Subject);
        }

        [Fact]
        public void StudentClassEndsWithLetter()
        {
            var students = DataManager.LoadStudents();
            foreach (var s in students)
                Assert.Matches(@"[A-ZÀ-ß]$", s.Class);
        }

        [Fact]
        public void TeacherFullNameDoesNotContainDigit()
        {
            var teachers = DataManager.LoadTeachers();
            foreach (var t in teachers)
                Assert.DoesNotMatch(@"\d", t.FullName);
        }

        [Fact]
        public void AllStudentsAreUnique()
        {
            var students = DataManager.LoadStudents();
            var set = new HashSet<string>();
            foreach (var s in students)
                Assert.True(set.Add(s.Id), $"Duplicate student: {s.Id}");
        }

        [Fact]
        public void AllGradesAreNotNull()
        {
            var grades = DataManager.LoadGrades();
            foreach (var g in grades)
                Assert.NotNull(g);
        }

        [Fact]
        public void StudentsAreSubsetOfAllPeople()
        {
            var students = DataManager.LoadStudents();
            var teachers = DataManager.LoadTeachers();
            var allPeople = students.Select(s => s.Id).Concat(teachers.Select(t => t.Id)).ToHashSet();
            foreach (var s in students)
                Assert.Contains(s.Id, allPeople);
        }

        [Fact]
        public void GradesContainSpecificMark()
        {
            var grades = DataManager.LoadGrades();
            Assert.Contains(grades, g => g.Mark == 5);
        }
    }
}
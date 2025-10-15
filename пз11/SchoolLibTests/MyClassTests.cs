using Xunit;
using SchoolLib;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SchoolLibTests
{
    public class MyClassTests
    {
        [Fact]
        public void TestContains()
        {
            var mcl = new MyClass();
            string full = mcl.SayHello("Harry Potter");
            Assert.Contains("Potter", full);
        }

        [Fact]
        public void TestStartsWith()
        {
            var mcl = new MyClass();
            string full = mcl.SayHello("Donald Duck");
            Assert.StartsWith("Hello Donald", full);
        }

        [Fact]
        public void TestEndsWith()
        {
            var mcl = new MyClass();
            string full = mcl.SayHello("Uncle Scrooge");
            Assert.EndsWith("Scrooge", full);
        }

        [Fact]
        public void TestMatches()
        {
            var mcl = new MyClass();
            string full = mcl.SayHello("Test123");
            Assert.Matches(@"\d+", full);
        }

        [Fact]
        public void TestAreEqualVsAreSame()
        {
            string expected = "a";
            string actual = string.Copy(expected);
            Assert.Equal(expected, actual);    // success
            Assert.False(object.ReferenceEquals(expected, actual));  // success

            expected = "Hello";
            actual = string.Intern("Hello");
            Assert.True(object.ReferenceEquals(expected, actual));   // success
        }

        [Fact]
        public void TestAllItemsAreUnique()
        {
            var items = MyClass.GenerateItems();
            var set = new HashSet<string>();
            foreach (var item in items)
                Assert.True(set.Add(item.Name), $"Duplicate item: {item.Name}");
        }

        [Fact]
        public void TestAllItemsAreNotNull()
        {
            var items = MyClass.GenerateItems();
            foreach (var item in items)
                Assert.NotNull(item);
        }
    }
}
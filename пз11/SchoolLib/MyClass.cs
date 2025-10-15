using System;
using System.Collections.Generic;

namespace SchoolLib
{
    public class Item
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
    }

    public class MyClass
    {
        public string SayHello(string name)
        {
            if (name == null)
                throw new ArgumentNullException("Имя параметра не может быть пустым");
            return "Hello " + name;
        }

        public static List<Item> GenerateItems()
        {
            return new List<Item>
            {
                new Item { Name = "Apple", Quantity = 5 },
                new Item { Name = "Banana", Quantity = 1 },
                new Item { Name = "Orange", Quantity = 3 }
            };
        }
    }
}
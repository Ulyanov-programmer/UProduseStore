using System;
using UlyanovProduseStore.BL.Controller;
using UlyanovProduseStore.BL.Model;

namespace UlyanovProduseStore.WarehouseTerminal
{
    class Program
    {
        static void Main(string[] args)
        {
            Employee newEmployee = new Employee("Сотрудник");
            Console.WriteLine("Здравствуйте сотрудник, введите ваш ID.");

            int.TryParse(Console.ReadLine(), out int inputID);

            if (inputID == newEmployee.GetID())
            {
                while (true)
                {
                    Console.Clear();
                    Console.WriteLine("Вам доступно добавление продуктов.");
                    ProductController.AddProducts(newEmployee);
                }
            }
        }
    }
}

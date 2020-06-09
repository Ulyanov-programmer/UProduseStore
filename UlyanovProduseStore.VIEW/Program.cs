using System;
using UlyanovProduseStore.BL.Controller;

namespace UlyanovProduseStore.VIEW
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Здравствуйте! Нам доступны следующие продукты:");


            if (ProductController.ShowProducts().Count == 0)
            {
                Console.WriteLine("К сожалению, список продуктов пуст.");
            }

        }
    }
}

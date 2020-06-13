using System;
using UlyanovProduseStore.BL.Controller;
using UlyanovProduseStore.BL.Model;

namespace UlyanovProduseStore.VIEW
{
    class Program
    {
        static void Main(string[] args)
        {
            Client client = new Client("Новый пользователь");

            if (ClientController.FindClient(client) == false)
            {
                Console.WriteLine("Вы не зарегистрированы и ранее не заходили, данные о вас сгенерированы и сохранены по умолчанию.");
            }
            
            Console.WriteLine("Здравствуйте! Нам доступны следующие продукты:");


            if (ProductController.ShowProducts().Count == 0) //TODO: Накидать продуктов (создать метод для этого) в файл.
            {
                Console.WriteLine("К сожалению, список продуктов пуст.");
            }
            else
            {
                Console.WriteLine("Введите e(англ) для того, что-бы занести продукт в корзину. Регистр не учитывается.");
                Console.WriteLine("Или введите q, если хотите пополнить счёт.");
                var inputKey = Console.ReadLine().ToLower();
                if (inputKey == "e")
                {
                    ClientController.Buy(client);
                }
                else if (inputKey == "q")
                {
                    ClientController.UpBalance(client, 1000); //1000 - временная затычка.
                }
            }
            
        }
    }
}

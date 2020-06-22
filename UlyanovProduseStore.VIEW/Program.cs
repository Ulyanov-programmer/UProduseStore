using System;
using UlyanovProduseStore.BL.Controller;
using UlyanovProduseStore.BL.Model;
using System.Linq;

namespace UlyanovProduseStore.VIEW
{
    class Program
    {
        static void Main(string[] args)
        {
            Client client = new Client("Новый пользователь");
            if (ClientController.FindPerson(client) == false)
            {
                Console.WriteLine("Вы не зарегистрированы и ранее не заходили, данные о вас сгенерированы и сохранены по умолчанию.");
            }
            //TODO: Создать более полноценное меню.

            Console.WriteLine($"Здравствуйте, {client.ToString()} Нам доступны следующие продукты:");
            var products = ProductController.ShowProducts();

            if (products.Count == 0)
            {
                Console.WriteLine("К сожалению, список продуктов пуст.");
            }
            else
            {
                foreach (var product in products)
                {
                    Console.Write($"{ProductController.GetName(product)}, "  );
                    Console.Write($"стоимость: {ProductController.GetCost(product)} рублей, ");
                    Console.WriteLine($"категория {ProductController.GetCategory(product)}.");
                }
                Console.WriteLine("Введите e(англ) и нажмите Enter, если хотите добавить продукт (выбрать чуть позже) в корзину.");
                Console.WriteLine("Или введите q и нажмите Enter, если хотите пополнить счёт.");
                var inputKey = Console.ReadLine().ToLower();
                if (inputKey == "e")
                {
                    Console.WriteLine("Введите полное название продукта."); 
                    var product = products.SingleOrDefault(x => ProductController.GetName(x) == Console.ReadLine());
                    ClientController.AddProductInBasket(client, product);
                }
                else if (inputKey == "q")
                {
                    Console.WriteLine("Введите сумму пополнения.");
                }
            }
            
        }
    }
}

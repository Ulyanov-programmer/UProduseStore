using System;
using UlyanovProduseStore.BL.Controller;
using UlyanovProduseStore.BL.Model;
using System.Linq;
using System.Threading;

namespace UlyanovProduseStore.VIEW
{
    class Program
    {
        static void Main(string[] args)
        {
            string name;

            while (true)
            {
                Console.WriteLine("Здравствуйте уважаемый пользователь! Введите своё имя что-бы войти.");
                Console.WriteLine("(если ранее не заходили, введите ваше будущее имя)");
                //TODO: Сделать предложение переписать имя.
                name = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(name))
                {
                    Console.WriteLine("Имя не может быть пустым или состоять только из пробелов или символов разделителей!");
                    continue;
                }
                break;
            }
            var client = ClientController.FindPerson<Client>(name) as Client;

            var products = ProductController.LoadProducts();

            if (products.Count == 0)
            {
                Console.WriteLine("К сожалению, список продуктов пуст. Возвращайтесь позже. ");
                Console.Read();
                //TODO: Изменить логику так, что бы в этом случае была возможность только пополнить счёт. 
            }
            else
            {
                while (true)
                {
                    Console.Clear();
                    Console.WriteLine("Нам доступны следующие продукты:");

                    foreach (var product in products)
                    {
                        Console.WriteLine($"{ProductController.GetName(product)}, ");
                        Console.WriteLine($"стоимость: {ProductController.GetCost(product)} рублей, ");
                        Console.WriteLine($"категория {ProductController.GetCategory(product)}. \n");
                    }

                    Console.WriteLine("\nНажмите E(англ), если хотите добавить один из продуктов в корзину.");
                    Console.WriteLine("Или нажмите Q, если хотите пополнить счёт.");
                    Console.WriteLine("Если ваша корзина уже заполнена, нажмите Y, что бы совершить покупку.");
                    Console.WriteLine("(раскладка не учитывается)");
                    //TODO: Добавить сохранение чека с информацией о покупке.

                    var inputKey = Console.ReadKey().Key;
                    switch (inputKey)
                    {
                        case ConsoleKey.E:
                            Console.WriteLine("\nВведите полное название продукта.");
                            string inputNameOfProduct = Console.ReadLine();

                            var product = products.SingleOrDefault(x => ProductController.GetName(x) == inputNameOfProduct);
                            if (product == default)
                            {
                                Console.WriteLine("Продукта с таким именем не существует в продаже!");
                                break;
                            }
                            ClientController.AddProductInBasket(client, product);
                            //TODO: Добавить метод удаления продуктов из корзины.

                            break;

                        case ConsoleKey.Q:
                            Console.Write("\nСумма пополнения в рублях: ");
                            if (decimal.TryParse(Console.ReadLine(), out decimal input) == false)
                            {
                                Console.WriteLine("Были введены некорректные данные!");
                                Thread.Sleep(3000);
                                Console.Clear();
                                continue;
                            }
                            if (ClientController.UpBalance(client, input))
                            {
                                Console.WriteLine($"Ваш баланс пополнен на {input} рублей и теперь составляет {ClientController.GetBalance(client)} рублей.");
                                Thread.Sleep(6000);
                            }
                            break;

                        case ConsoleKey.Y:
                            Console.Clear();
                            Console.WriteLine(@"Вы уверены, что хотите совершить покупку? Введите ""да"", если согласны.");
                            Console.WriteLine("(регистр не учитывается)");

                            if (Console.ReadLine().ToLower() == "да")
                            {
                                if (ClientController.Buy(client))
                                {
                                    Console.WriteLine("Покупка успешно совершена. Ваши данные после покупки:");
                                    Console.WriteLine($"Баланс: {ClientController.GetBalance(client)}");
                                    Console.WriteLine($"Коэффициент скидки:{ClientController.GetDiscountRate(client)}");
                                }
                                else
                                {
                                    Console.Write("Ваш баланс меньше чем общая стоимость корзины с учётом коэффициента скидки,");
                                    Console.WriteLine("ваш аккаунт повреждён или корзина пуста!");
                                }
                            }
                            break;
                    }
                }
            }
        }
    }
}

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
            string password;
            Client сlient = default;
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Здравствуйте уважаемый пользователь! Если вы зарегистрированы, введите E и введите свои данные.");
                Console.WriteLine("Или если вы не зарегистрированы, введите R и введите данные о вас.");
                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.R:
                        Console.Write("\nВаше новое имя: ");
                        name = Console.ReadLine();
                        Console.Write("Ваш новый пароль: ");
                        password = Console.ReadLine();
                        сlient = ClientController.RegistrationOfPerson<Client>(name, password, "X", UPSClientContext.StringConnectToMainClientServer) as Client;

                        if (сlient == null)
                        {
                            Console.Write("Данные некорректны (пустое имя/пароль/в них только символы разделители),");
                            Console.WriteLine(" или пользователь с таким именем уже существует!");
                            Thread.Sleep(6000);
                            continue;
                        }

                        break;

                    case ConsoleKey.E:
                        Console.Write("\nИмя: ");
                        name = Console.ReadLine();
                        Console.Write("Пароль: ");
                        password = Console.ReadLine();
                        сlient = ClientController.LoadOfPerson<Client>(name, password) as Client;

                        if (сlient == null)
                        {
                            Console.Write("Данные некорректны (пустое имя/пароль/в них только символы разделители или они неверны).");
                            Thread.Sleep(6000);
                            continue;
                        }

                        break;

                    default:
                        continue;
                }
                Console.Clear();
                break;
            }
            var products = ProductController.LoadProducts(UPSEmployeeContext.StringConnectToEmployeeServer);

            while (true)
            {
                Console.Clear();
                if (products.Count > 0)
                {
                    Console.WriteLine("Нам доступны следующие продукты:");
                    foreach (var product in products)
                    {
                        Console.WriteLine($"{ProductController.GetName(product)}, ");
                        Console.Write($"стоимость: {ProductController.GetCost(product)} рублей. \n\n");
                    }
                    Console.WriteLine("Нажмите E(англ), если хотите добавить один из продуктов в корзину.");
                    Console.WriteLine("Если ваша корзина уже заполнена, нажмите Y, что бы совершить покупку.");
                    Console.WriteLine("Если хотите удалить продукт из корзины, нажмите I, после чего вы введёте его имя.");
                }
                Console.WriteLine("Нажмите Q, если хотите пополнить счёт.");
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
                        //ClientController.AddProductInBasket(сlient, product);
                        Console.WriteLine($"Продукт {inputNameOfProduct} добавлен в корзину!");
                        Thread.Sleep(6000);
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
                        if (ClientController.UpBalance(сlient, input, UPSClientContext.StringConnectToMainClientServer))
                        {
                            Console.WriteLine($"Ваш баланс пополнен на {input} рублей и теперь составляет {ClientController.GetBalance(сlient)} рублей.");
                            Thread.Sleep(6000);
                        }
                        break;

                    case ConsoleKey.Y:
                        Console.Clear();
                        Console.WriteLine(@"Вы уверены, что хотите совершить покупку? Введите ""да"", если согласны.");
                        Console.WriteLine("(регистр не учитывается)");

                        if (Console.ReadLine().ToLower() == "да")
                        {
                            //if (ClientController.Buy(сlient))
                            //{
                            //    Console.WriteLine("Покупка успешно совершена. Ваши данные после покупки:");
                            //    Console.WriteLine($"Баланс: {ClientController.GetBalance(сlient)}");
                            //    Console.WriteLine($"Коэффициент скидки:{ClientController.GetDiscountRate(сlient)}");
                            //    Thread.Sleep(6000);
                            //}
                            //else
                            //{
                            //    Console.Write("Ваш баланс меньше чем общая стоимость корзины с учётом коэффициента скидки,");
                            //    Console.Write("ваш аккаунт повреждён или корзина пуста! \n");
                            //}
                            Thread.Sleep(6000);
                        }
                        break;
                }
            }
        }
    }
}

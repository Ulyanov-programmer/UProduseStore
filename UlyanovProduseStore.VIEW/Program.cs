using System;
using UlyanovProduseStore.BL.Controller;
using UlyanovProduseStore.BL.Model;
using System.Linq;
using System.Threading;
using UlyanovProduseStore.BL;

namespace UlyanovProduseStore.VIEW
{
    class Program
    {
        static void Main()
        {
            UPSContext context = new UPSContext(UPSContext.StringConnectToMainServer);
            Client сlient = default;
            Basket basket = new Basket();

            #region Authorization

            while (true)
            {
                Console.Clear();
                Console.WriteLine("Здравствуйте уважаемый пользователь! Если вы зарегистрированы, введите L и введите свои данные.");
                Console.WriteLine("Или если вы не зарегистрированы, введите R и введите данные о вас.");
                switch (Console.ReadKey(true).Key)
                {
                    #region key L (log-in)

                    case ConsoleKey.L:
                        Console.Write("Имя: ");
                        string newName = Console.ReadLine();
                        Console.Write("Пароль: ");
                        string newPassword = Console.ReadLine();

                        сlient = ClientController.LoadOfPerson<Client>(newName, newPassword, context) as Client;

                        if (сlient is null)
                        {
                            Console.Write("Данные некорректны (пустое имя/пароль/в них только символы разделители или они неверны).");
                        }
                        else
                        {
                            basket.Client = сlient;
                            Console.WriteLine($"Добро пожаловать, {сlient}!");
                        }
                        Thread.Sleep(6000);

                        break;

                    #endregion

                    #region key R (registration)

                    case ConsoleKey.R:
                        Console.Write("Ваше новое имя: ");
                        string name = Console.ReadLine();
                        Console.Write("Ваш новый пароль: ");
                        string password = Console.ReadLine();

                        сlient = ClientController.RegistrationOfPerson<Client>(name, password, context) as Client;

                        if (сlient is null)
                        {
                            Console.Write("Данные некорректны (пустое имя/пароль/в них только символы разделители), ");
                            Console.WriteLine("или пользователь с таким именем уже существует!");
                            Thread.Sleep(6000);
                            continue;
                        }
                        basket.Client = сlient;

                        break;

                     #endregion
                }
                break;
            }

            #endregion


            var products = ProductController.LoadProducts(context);

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
                        //TODO: изменить логику так, что-бы подробная информация выводилась только при вводе имени продукта.
                    }
                    Console.WriteLine("Нажмите A(англ), если хотите добавить один из продуктов в корзину.");
                    Console.WriteLine("Если ваша корзина уже заполнена, нажмите Y, что бы совершить покупку.");
                    Console.WriteLine("Если хотите удалить продукт из корзины, нажмите I, после чего вы введёте его имя.");
                }
                Console.WriteLine("Нажмите Q, если хотите пополнить счёт.");
                Console.WriteLine("(раскладка не учитывается)");
                //TODO: Добавить сохранение чека с информацией о покупке.

                switch (Console.ReadKey(true).Key)
                {
                    #region key E (adding product)

                    case ConsoleKey.E:
                        Console.WriteLine("Введите полное название продукта.");
                        string inputNameOfProduct = Console.ReadLine();

                        var product = products.SingleOrDefault(prod => prod.Name == inputNameOfProduct);
                        if (product != default)
                        {
                            if (ClientController.AddProductInBasket(basket, product))
                            {
                                Console.WriteLine($"Продукт с именем {product} добавлен в корзину!");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Продукта с таким именем не существует в продаже!");
                        }
                        Thread.Sleep(6000);

                        break;

                    #endregion

                    #region key Q (upping balance)

                    case ConsoleKey.Q:
                        Console.Write("Сумма пополнения в рублях: ");

                        if (decimal.TryParse(Console.ReadLine(), out decimal input) == false)
                        {
                            Console.WriteLine("Были введены некорректные данные!");
                            Thread.Sleep(3000);
                            continue;
                        }
                        if (ClientController.UpBalance(сlient, input, context))
                        {
                            Console.WriteLine($"Ваш баланс пополнен на {input} рублей и теперь составляет {ClientController.GetBalance(сlient)} рублей.");
                            Thread.Sleep(6000);
                        }
                        break;

                    #endregion

                    #region key Y (buy)

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
                            //Thread.Sleep(6000);
                        }
                        break;

                     #endregion
                }
            }
        }
    }
}

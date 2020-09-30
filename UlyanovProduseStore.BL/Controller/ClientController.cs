using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UlyanovProduseStore.BL.Model;

namespace UlyanovProduseStore.BL.Controller
{
    /// <summary>
    /// Класс-контроллер, содержащий методы для работы с экземплярами Client и Basket.
    /// </summary>
    public class ClientController
    {
        /// <summary>
        /// Добавляет экземпляр Product в объект Basket.
        /// </summary>
        /// <param name="basket"> Объект Basket, в который будет произведено добавление. </param>
        /// <param name="product"> Экземпляр Product, который будет добавлен.</param>
        /// <returns> True - если входные аргументы корректны и операция удалась, иначе - false. </returns>
        public static bool AddProductInBasket(Basket basket, Product product)
        {
            if (product != null && basket != null)
            {
                basket.Products.Add(product);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Выполняет операцию покупки, удаляя из экземпляра Basket все объекты Product
        /// и вычитает их сумму их баланса объекта Client, который содержится в Basket.
        /// </summary>
        /// <param name="basket"> Объект Basket. </param>
        /// <param name="context"> Экземпляр контекста, необходимый для сохранения изменений в базе данных. </param>
        /// <returns> True - если входные аргументы корректны и операция удалась, иначе - false.  </returns>
        public static bool Buy(Basket basket, UPSContext context)
        {
            if (basket != null &&
                basket.Client != null &&
                basket.Products != null &&
                basket.Products.Count > 0)
            {
                var sumCost = basket.Products.Select(x => x.Cost)
                                             .Sum();

                if (basket.Client.Balance >= sumCost)
                {
                    basket.Client.Balance -= sumCost;

                    basket.Products = new List<Product>();
                    SaveClient(basket.Client, context);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Загружает объект, наследника Person, из БД.
        /// </summary>
        /// <typeparam name="T"> Тип выгружаемого объекта. Должен наследоваться от Person. </typeparam>
        /// <param name="nameOfPerson"> Имя выгружаемого объекта. </param>
        /// <param name="passwordOrSecondName"> Пароль или фамилия выгружаемого объекта. </param>
        /// <param name="context"> Экземпляр контекста, необходимый для работы с БД. </param>
        /// <returns> Если входные аргументы корректны и операция удалась - возвращает загруженный объект, 
        ///           иначе - null.
        /// </returns>
        public static Person LoadOfPerson<T>(string nameOfPerson, string passwordOrSecondName, UPSContext context) where T : Person
        {
            if (string.IsNullOrWhiteSpace(nameOfPerson) == false &&
                string.IsNullOrWhiteSpace(passwordOrSecondName) == false)
            {
                var nameOfType = typeof(T);

                try
                {
                    if (nameOfType == typeof(Client))
                    {
                        var clientFromDb = context.Clients.FirstOrDefault(clint => clint.Name == nameOfPerson &&
                                                                                   clint.PasswordOrSecondName == passwordOrSecondName);
                        if (clientFromDb != default)
                        {
                            return clientFromDb;
                        }
                    }
                    else if (nameOfType == typeof(Employee))
                    {
                        var employeeFromDb = context.Employees.FirstOrDefault(emp => emp.Name == nameOfPerson &&
                                                                                   emp.PasswordOrSecondName == passwordOrSecondName);
                        if (employeeFromDb != default)
                        {
                            return employeeFromDb;
                        }
                    }
                    else
                        return null;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return null;
                }
            }
            return null;
        }

        /// <summary>
        /// Добавляет в БД объект, наследника от Person.
        /// </summary>
        /// <typeparam name="T"> Тип выгружаемого объекта. Должен наследоваться от Person. </typeparam>
        /// <param name="nameOfPerson"> Имя выгружаемого объекта.  </param>
        /// <param name="passwordOrSecondName"> Пароль или фамилия выгружаемого объекта. </param>
        /// <param name="context"> Экземпляр контекста, необходимый для работы с БД. </param>
        /// <returns> Если входные аргументы корректны и операция удалась - возвращает загруженный объект, 
        ///           иначе - null.
        /// </returns>
        public static Person RegistrationOfPerson<T>(string nameOfPerson, string passwordOrSecondName, UPSContext context)
        {
            if (string.IsNullOrWhiteSpace(nameOfPerson) is false &&
                string.IsNullOrWhiteSpace(passwordOrSecondName) is false)
            {
                var nameOfPersonType = typeof(T);
                try
                {
                    if (nameOfPersonType == typeof(Client))
                    {
                        if (context.Clients.FirstOrDefault(clint => clint.Name == nameOfPerson) == default)
                        {
                            var newClient = new Client(nameOfPerson, passwordOrSecondName);

                            if (newClient.Name != null &&
                                newClient.PasswordOrSecondName != null)
                            {
                                context.Clients.Add(newClient);
                                context.SaveChanges();
                                return newClient;
                            }
                        }
                    }
                    else if (nameOfPersonType == typeof(Employee))
                    {
                        if (context.Employees.FirstOrDefault(clint => clint.Name == nameOfPerson) == default)
                        {
                            var newEmployee = new Employee(nameOfPerson, passwordOrSecondName);

                            if (newEmployee.Name != null &&
                                newEmployee.PasswordOrSecondName != null)
                            {
                                context.Employees.Add(newEmployee);
                                context.SaveChanges();
                                return newEmployee;
                            }
                        }
                    }
                    else
                        return null;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return null;
                }
            }
            return null;
        }

        /// <summary>
        /// Увеличивает баланс экземпляра Client и сохраняет изменения.
        /// </summary>
        /// <param name="client"> Экземпляр Client, для которого будет произведено пополнение. </param>
        /// <param name="money"> Сумма пополнения. </param>
        /// <returns> Если входные аргументы корректны и операция удалась, возвращает текущий баланс входного Client.
        ///           Иначе возвращает -1. </returns>
        public static decimal UpBalance(Client client, decimal money, UPSContext context)
        {
            //Да, да, тут должна быть переадресация на платёжные системы и т.д.
            if (money > 0 && client != null)
            {
                client.Balance += money;

                SaveClient(client, context);
                return client.Balance;
            }
            else
            {
                Console.WriteLine("Сумма пополнения менее или равна нулю или аккаунт повреждён!");
                return -1;
            }
        }

        /// <summary>
        /// Приватный метод, необходимый для сохранения (синхронизации) данных об экземпляре Client после операций.
        /// </summary>
        /// <param name="inputClient"> Экземпляр Client, данные которого будут синхронизированы с БД. </param>
        /// <param name="context"> Экземпляр контекста, необходимый для работы с БД. </param>
        private static void SaveClient(Client inputClient, UPSContext context)
        {
            try
            {
                var clientFromDb = context.Clients.Find(inputClient.Id);
                clientFromDb = inputClient;
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Не удалось сохранить данные после операции!" + ex);
                Thread.Sleep(5000);
            }
        }

        /// <summary>
        /// Удаляет экземпляр Product из входного объекта Basket. Внимание, будет удалено первое вхождение этого экземпляра Product.
        /// </summary>
        /// <param name="basket"> Объект Basket, из которого будет произведено удаление. </param>
        /// <param name="nameOfTheProductBeDeleted"> Имя экземпляра Product, первое вхождение которого будет удалено. </param>
        /// <returns> True - если входные аргументы корректны и операция удалась, иначе - false. </returns>
        public static bool DeleteProductFromBasket(Basket basket, string nameOfTheProductBeDeleted)
        {
            if (basket != null &&
                basket.Client != null &&
                basket.Products != null &&
                basket.Products.Count > 0 &&
                basket.Products.Any(prod => prod.Name == nameOfTheProductBeDeleted))
            {
                int index = basket.Products.FindIndex(prod => prod.Name == nameOfTheProductBeDeleted);
                basket.Products.RemoveAt(index);

                return true;
            }
            return false;
        }

        #region GettersSetters

        /// <summary>
        /// Возвращает баланс экземпляра класса Client.
        /// </summary>
        /// <param name="client">Клиент из которого будет производится чтение.</param>
        /// <returns></returns>
        public static decimal GetBalance(Client client)
        {
            return client.Balance;
        }

        /// <summary>
        /// Возвращает пароль входного пользователя. 
        /// </summary>
        /// <param name="client">Клиент из которого будет производится чтение.</param>
        /// <returns>Если клиент пуст или его поле "Password" пусто, возвращает null. Иначе - содержимое поля Password.</returns>
        public static string GetPassword(Client client)
        {
            if (client != null || client.PasswordOrSecondName != null)
            {
                return client.PasswordOrSecondName;
            }
            else
            {
                return null;
            }
        }

        #endregion
    }
}

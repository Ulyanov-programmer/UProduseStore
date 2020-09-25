using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UlyanovProduseStore.BL.Model;

namespace UlyanovProduseStore.BL.Controller
{
    public class ClientController
    {
        public static bool AddProductInBasket(Basket basket, Product product)
        {
            if (product != null && basket != null)
            {
                basket.Products.Add(product);
                return true;
            }
            return false;
        }

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
                                                                                   clint.Password == passwordOrSecondName);
                        if (clientFromDb != default)
                        {
                            return clientFromDb;
                        }
                    }
                    else if (nameOfType == typeof(Employee))
                    {
                        var employeeFromDb = context.Clients.FirstOrDefault(emp => emp.Name == nameOfPerson &&
                                                                                   emp.Password == passwordOrSecondName);
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
                        var newClient = new Client(nameOfPerson, passwordOrSecondName);
                        if (newClient.Name != null && newClient.Password != null)
                        {
                            context.Clients.Add(newClient);
                            context.SaveChanges();
                            return newClient;
                        }
                    }
                    else if (nameOfPersonType == typeof(Employee))
                    {
                        var newEmployee = new Employee(nameOfPerson, passwordOrSecondName);
                        if (newEmployee.Name != null && newEmployee.SecondName != null)
                        {
                            context.Employees.Add(newEmployee);
                            context.SaveChanges();
                            return newEmployee;
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
        /// Увеличивает баланс экземпляра класса Client и сохраняет изменения клиента.
        /// </summary>
        /// <param name="client">Экземпляр класса Client, для которого будет произведено пополнение. </param>
        /// <param name="money">Сумма пополнения. </param>
        /// <returns> Возвращает true, если client не равен null, money более нуля и пополнение было успешно совершено.
        ///           Иначе - возвращает false. </returns>
        public static bool UpBalance(Client client, decimal money, UPSContext context)
        {
            //Да, да, тут должна быть переадресация на платёжные системы и т.д.
            if (money > 0 && client != null)
            {
                client.Balance += money;

                SaveClient(client, context);
                return true;
            }
            else
            {
                Console.WriteLine("Сумма пополнения менее или равна нулю или аккаунт повреждён!");
                return false;
            }
        }

        
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
            if (client != null || client.Password != null)
            {
                return client.Password;
            }
            else
            {
                return null;
            }
        }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UlyanovProduseStore.BL.Model;

namespace UlyanovProduseStore.BL.Controller
{
    public class ClientController
    {
        /// <summary>
        /// Добавляет в поле BasketOfproducts экземпляра Client объект Product и сохраняет изменения Client.
        /// </summary>
        /// <param name="client">Клиент в "корзину" которого будет добавлен продукт.</param>
        /// <param name="product">Объект Product.</param>
        public static bool AddProductInBasket(Client client, Product product)
        {
            if (product != null && client != null)
            {
                client.BasketOfproducts.Add(product);
                SaveClient(client);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Считывает с счёта клиента стоимость всех продуктов в его корзине умноженную на коэффициент скидки клиента,
        /// после чего изменяет коэффициент скидки, очищает (придаёт базовое представление) листу продуктов и сохраняет изменения клиента.
        /// </summary>
        /// <param name="client">Объект класса Client. </param>
        /// <returns> True - если счёт клиента больше или равен сумме стоимости продуктов и была проведена операция вычитания. 
        ///           False - если счёт клиента меньше суммы стоимости продуктов или клиент пуст или корзина пуста. </returns>
        public static bool Buy(Client client)
        {
            if (client != null && client.BasketOfproducts.Count > 0)
            {
                var SumCost = client.BasketOfproducts.Select(x => x.Cost)
                                                     .Sum();

                SumCost = SumCost * (decimal)client.DiscountRate;

                if (client.Balance >= SumCost)
                {
                    client.Balance -= SumCost;
                    client.DiscountRate -= (double)SumCost / 100000;
                    client.DiscountRate = Math.Round(client.DiscountRate, 2); //TODO: Некорректное округление, исправить.

                    if (client.DiscountRate < 0.90)
                    {
                        client.DiscountRate = 0.90;
                    }
                    client.BasketOfproducts = new List<Product>();
                    SaveClient(client);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Метод поиска данных о пользователе среди сериализованных их версий. Открывает файл с данными по пути указанному 
        /// в константе Person.PathSaveOfPersons, открывает или создаёт файл и проверяет на заполненность.
        /// </summary>
        /// <typeparam name="T">Тип для определения логики метода (ограничен Person и его наследниками).</typeparam>
        /// <returns> 
        /// Если файл был заполнен - создаёт на основе файла и возвращает экземпляр класса Client или Employee (в зависимости от значения T).  
        /// Иначе - создаёт новый экземпляр класса Client или Employee (в зависимости от значения T) и возвращает его.
        /// </returns>
        public static Person FindPerson<T>(string nameOfPerson) where T: Person
        {
            var binFormatter = new BinaryFormatter();

            if (File.Exists(Person.PathSaveOfPersons))
            {
                Directory.CreateDirectory(Person.PathSaveOfPersons);
            }
            try
            {
                using (var stream = new FileStream($@"{Person.PathSaveOfPersons}\user{nameOfPerson}.dat", FileMode.OpenOrCreate))
                {
                    if (stream.Length == 0)
                    {
                        if (typeof(T) == typeof(Client))
                        {
                            Client newClient = new Client(nameOfPerson);
                            binFormatter.Serialize(stream, newClient);
                            return newClient;
                        }
                        else if (typeof(T) == typeof(Employee))
                        {
                            Employee newEmployee = new Employee(nameOfPerson);
                            binFormatter.Serialize(stream, newEmployee);
                            return newEmployee;
                        }
                    }
                    return binFormatter.Deserialize(stream) as T;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("Не удалось получить данные о вас или файл о вас повреждён!");
            }
        }

        /// <summary>
        /// Увеличивает баланс экземпляра класса Client и сохраняет изменения клиента.
        /// </summary>
        /// <param name="client">Экземпляр класса Client, для которого будет произведено пополнение. </param>
        /// <param name="money">Сумма пополнения. </param>
        /// <returns> Возвращает true, если client не равен null, money более нуля и пополнение было успешно совершено.
        ///           Иначе - возвращает false. </returns>
        public static bool UpBalance(Client client, decimal money)
        {
            //Да, да, тут должна быть переадресация на платёжные системы и т.д.
            if (money > 0 && client != null)
            {
                client.Balance += money;
                SaveClient(client);
                return true;
            }
            else
            {
                Console.WriteLine("Сумма пополнения менее или равна нулю или аккаунт повреждён!");
                return false;
            }
        }

        /// <summary>
        /// Приватный метод, нужный для сохранения данных после операций.
        /// </summary>
        /// <param name="client">Сохраняемый клиент. </param>
        private static void SaveClient(Client client)
        {
            var bformatter = new BinaryFormatter();

            using (var stream = new FileStream($@"{Person.PathSaveOfPersons}\user{client.Name}.dat", FileMode.Create))
            {
                bformatter.Serialize(stream, client);
            }
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
        /// Возвращает коэффициент скидки экземпляра Client, хранящийся в формате "X.XX".
        /// </summary>
        /// <param name="client">Клиент из которого будет производится чтение.</param>
        /// <returns></returns>
        public static double GetDiscountRate(Client client)
        {
            return client.DiscountRate;
        }
        /// <summary>
        /// Возвращает корзину (лист) продуктов экземпляра класса Client.
        /// </summary>
        /// <param name="client">Клиент из которого будет производится чтение.</param>
        /// <returns></returns>
        public static List<Product> GetListOfProduct(Client client)
        {
            return client.BasketOfproducts;
        }
        #endregion
    }
}

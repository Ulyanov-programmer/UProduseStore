using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
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
            //TODO: Добавить проверку на наличие продуктов в файле/БД.
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
        /// в константе Person.PathSaveOfPersons, проверяет на заполненность.
        /// </summary>
        /// <typeparam name="T">Тип возвращаемого значения (ограничен Person и его наследниками).</typeparam>
        /// <param name="nameOfPerson">Имя пользователя. </param>
        /// <param name="inputPasswordOrID">Пароль или ID пользователя (если ищется экземпляр Employee - впишите любое значение).</param>
        /// <returns> 
        /// Если файл существует и был заполнен (там сохранён класс T) - возвращает экземпляр класса Client или Employee (в зависимости от значения T).
        /// Иначе - возвращает null.
        /// </returns>
        public static Person LoadOfPerson<T>(string nameOfPerson, string inputPasswordOrID) where T : Person
        {
            var binFormatter = new BinaryFormatter();
            var nameOfType = typeof(T).Name;

            if (File.Exists(Person.PathSaveOfPersons))
            {
                Directory.CreateDirectory(Person.PathSaveOfPersons);
            }
            try
            {
                if (File.Exists($@"{Person.PathSaveOfPersons}\user{nameOfPerson}.dat"))
                {
                    using (var stream = new FileStream($@"{Person.PathSaveOfPersons}\user{nameOfPerson}.dat", FileMode.Open))
                    {
                        if (stream.Length == 0)
                        {
                            File.Delete($@"{Person.PathSaveOfPersons}\user{nameOfPerson}.dat");
                            return null;
                        }
                        else if (nameOfType == "Client")
                        {
                            var loadedClient = binFormatter.Deserialize(stream) as Client;
                            if (GetPassword(loadedClient) != inputPasswordOrID)
                            {
                                return null;
                            }
                            return loadedClient;
                        }
                        else if(nameOfType == "Employee")
                        {
                            var loadedEmployee = binFormatter.Deserialize(stream) as Employee;
                            return loadedEmployee;
                        }
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Сохраняет данные о пользователе и возвращает его.
        /// </summary>
        /// <typeparam name="T">Тип возвращаемого и сохраняемого значения </typeparam>
        /// <param name="nameOfPerson">Имя нового пользователя. </param>
        /// <param name="passwordOrID">Пароль или ID нового пользователя (если ищется экземпляр Employee - впишите любое значение).</param>
        /// <returns>
        /// Возвращает null, если входные имя или пароль являются null, пусты или состоят только из символов-разделителей, 
        /// если пользователь с таким именем уже существует, или если T это Person.
        /// В ином случае - сохраняет и возвращает новый экземпляр класса T.
        /// </returns>
        public static Person RegistrationOfPerson<T>(string nameOfPerson, string passwordOrID)
        {
            if (string.IsNullOrWhiteSpace(nameOfPerson) || string.IsNullOrWhiteSpace(passwordOrID))
            {
                return null;
            }
            if (File.Exists(Person.PathSaveOfPersons))
            {
                Directory.CreateDirectory(Person.PathSaveOfPersons);
            }
            if (File.Exists($@"{Person.PathSaveOfPersons}\user{nameOfPerson}.dat"))
            {
                return null;
            }
            try
            {
                var binFormatter = new BinaryFormatter();
                using (var stream = new FileStream($@"{Person.PathSaveOfPersons}\user{nameOfPerson}.dat", FileMode.Create))
                {
                    var nameOfPersonType = typeof(T);

                    if (nameOfPersonType == typeof(Client))
                    {
                        var newClient = new Client(nameOfPerson, passwordOrID);
                        if (newClient.ToString() != null && ClientController.GetPassword(newClient) != null)
                        {
                            binFormatter.Serialize(stream, newClient);
                            return newClient;
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else if (nameOfPersonType == typeof(Employee))
                    {
                        var newEmployee = new Employee(nameOfPerson);
                        if (newEmployee.ToString() != null)
                        {
                            binFormatter.Serialize(stream, newEmployee);
                            return newEmployee;
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else
                    {
                        throw new Exception("Указанный тип объекта не существует!");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
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
            try
            {
                using (var stream = new FileStream($@"{Person.PathSaveOfPersons}\user{client.Name}.dat", FileMode.Create))
                {
                    bformatter.Serialize(stream, client);
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Не удалось сохранить данные после операции!");
                Thread.Sleep(5000);
            }
        }

        /// <summary>
        /// Удаляет экземпляр класса Product из корзины входного Client. 
        /// Если таких экземпляров более одного, будет удалено первое вхождение, с соответствующим названием из nameOfTheProductBeDeleted.
        /// </summary>
        /// <param name="client">Экземпляр класса Client, из корзины которого будет удалён экземпляр Product.</param>
        /// <param name="NameOfTheProductBeDeleted">Имя удаляемого продукта.</param>
        /// <returns> Если client не пуст, не пуста его корзина, элемент с именем из входного NameOfTheProductBeDeleted есть в корзине,
        ///           и если удаление прошло успешно - возвращает true.
        ///           Иначе - false. 
        /// </returns>
        public static bool DeleteProductFromBasket(Client client, string nameOfTheProductBeDeleted)
        {
            if (client != null && client.BasketOfproducts.Count > 0 &&
                client.BasketOfproducts.Find(x => x.Name == nameOfTheProductBeDeleted) != default)
            {
                client.BasketOfproducts.RemoveAt(client.BasketOfproducts.FindIndex(x => x.Name == nameOfTheProductBeDeleted));

                return true;
            }
            else
            {
                return false;
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
        /// <summary>
        /// Возвращает пароль входного пользователя. 
        /// </summary>
        /// <param name="client">Клиент из которого будет производится чтение.</param>
        /// <returns></returns>
        public static string GetPassword(Client client)
        {
            if (client != null && client.Password != null)
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

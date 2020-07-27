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
        public static bool WriteProductInFileBasket(Client client, int idOfProduct, string pathLoadFromDB)
        {
            if (client != null && idOfProduct > 0 && string.IsNullOrWhiteSpace(pathLoadFromDB) == false)
            {
                var bFormatter = new BinaryFormatter();
                var basket_With_Products = new List<Product>();
                string Path = SavePathParse(client);

                using (var stream = new FileStream(Path, FileMode.OpenOrCreate))
                {
                    if (stream.Length != 0)
                    {
                        basket_With_Products = bFormatter.Deserialize(stream) as List<Product>;
                    }

                    using (var context = new UProduseStoreContext(pathLoadFromDB))
                    {
                        basket_With_Products.Add(context.Products.FirstOrDefault(prod => prod.Id == idOfProduct));
                    }

                    bFormatter.Serialize(stream, basket_With_Products);
                }
                return true;
            }
            return false;
        }
        public static bool WriteProductInFileBasket(Client client, List<Product> listOfProducts)
        {
            if (client != null && listOfProducts != null)
            {
                var bFormatter = new BinaryFormatter();
                var list_With_Products = new List<Product>();
                string Path = SavePathParse(client);

                using (var stream = new FileStream(Path, FileMode.OpenOrCreate))
                {
                    if (stream.Length != 0)
                    {
                        list_With_Products = bFormatter.Deserialize(stream) as List<Product>;
                    }
                    list_With_Products.AddRange(listOfProducts);
                    bFormatter.Serialize(stream, list_With_Products);
                }
                return true;
            }
            return false;
        }

        public static List<Product> ReadFileWithListOfProduct(Client client, bool thisListWillBeWrite)
        {
            if (client != null)
            {
                var bFormatter = new BinaryFormatter();
                string Path = SavePathParse(client);

                using (var stream = new FileStream(Path, FileMode.OpenOrCreate))
                {
                    if (stream.Length != 0)
                    {
                        var listWithProducts = bFormatter.Deserialize(stream) as List<Product>;

                        if (thisListWillBeWrite == true)
                        {
                            foreach (var product in listWithProducts)
                            {
                                Console.WriteLine($"ID - {product.Id}, имя - {product.Name}, стоимость - {product.Cost}.");
                            }
                        }
                        return listWithProducts;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Считывает с счёта клиента стоимость всех продуктов в его корзине умноженную на коэффициент скидки клиента,
        /// после чего изменяет коэффициент скидки, очищает (придаёт базовое представление) листу продуктов и сохраняет изменения клиента.
        /// </summary>
        /// <param name="inputNameOfClient">Объект класса Client. </param>
        /// <returns> True - если счёт клиента больше или равен сумме стоимости продуктов и была проведена операция вычитания. 
        ///           False - если счёт клиента меньше суммы стоимости продуктов или клиент пуст или корзина пуста. </returns>
        public static bool Buy(Client client, string pathToServer, bool isUsed_InTest)
        {
            if (client == null && string.IsNullOrWhiteSpace(pathToServer))
            {
                throw new ArgumentNullException("Клиент повреждён или путь к серверу неверен!");
            }

            List<Product> listWithProducts = new List<Product>();
            string Path = SavePathParse(client);
            Client loadedClient = default;
            try
            {
                using (var context = new UProduseStoreContext(pathToServer))
                {
                    loadedClient = context.Clients.FirstOrDefault(clientFromDB => clientFromDB.Name == client.Name);
                }
                using (var stream = new FileStream(Path, FileMode.OpenOrCreate))
                {
                    if (stream.Length > 0)
                    {
                        listWithProducts = new BinaryFormatter().Deserialize(stream) as List<Product>;
                    }
                }

                if (client != default && listWithProducts.Count != 0)
                {
                    string сonsentBuyProducts = "";
                    if (isUsed_InTest == true)
                    {
                        сonsentBuyProducts = "да";
                    }
                    else
                    {
                        Console.Write("Вы действительно собираетесь купить эти продукты? Их характеристики: ");
                        foreach (var product in listWithProducts)
                        {
                            Console.WriteLine(product.Name);
                            Console.WriteLine($"{product.Cost}\n");
                        }
                        Console.WriteLine(@"Введите ""да"" (регистр не учитывается), если хотите совершить покупку.");
                        сonsentBuyProducts = Console.ReadLine().ToLower();
                    }

                    if (сonsentBuyProducts == "да")
                    {
                        decimal sumCostOfBusket = listWithProducts.Sum(prod => prod.Cost);
                        if (loadedClient.Balance >= sumCostOfBusket)
                        {
                            //Типа отправка заявления о доставке этих продуктов туда-то. 
                            File.Delete(Path);
                            loadedClient.Balance -= sumCostOfBusket;
                            SavePerson(loadedClient, pathToServer);
                            return true;
                        }
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла ошибка: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Метод поиска данных о пользователе среди сериализованных их версий. Открывает файл с данными по пути указанному 
        /// в константе Person.PathSaveOfPersons, проверяет на заполненность.
        /// </summary>
        /// <typeparam name="T">Тип возвращаемого значения (ограничен Person и его наследниками).</typeparam>
        /// <param name="inputName">Имя пользователя. </param>
        /// <param name="inputPasswordOrID">Пароль или ID пользователя (если ищется экземпляр Employee - впишите любое значение).</param>
        /// <returns> 
        /// Если файл существует и был заполнен (там сохранён класс T) - возвращает экземпляр класса Client или Employee (в зависимости от значения T).
        /// Иначе - возвращает null.
        /// </returns>
        public static Person LoadOfPerson<T>(string inputName, string passwordOrID, string pathLoad) where T : Person //TODO: ДОПИЛИТЬ ОПИСАНИЯ!!11
        {
            var nameOfType = typeof(T);
            try
            {
                using (var context = new UProduseStoreContext(pathLoad))
                {
                    if (nameOfType == typeof(Client))
                    {
                        var loadedClient = context.Clients.FirstOrDefault(client => client.Name == inputName
                                                                                 && client.Password == passwordOrID);
                        return loadedClient;
                    }
                    else if (nameOfType == typeof(Employee))
                    {
                        Employee newEmployee = context.Employees.FirstOrDefault(DBEmployee => DBEmployee.Name == inputName);
                        return newEmployee;
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
        /// <param name="passwordOrID">Пароль или ID нового пользователя (пароль не может быть менее 5 символов).</param>
        /// <returns>
        /// Возвращает null, если входные имя или пароль являются null, пусты или состоят только из символов-разделителей, 
        /// если пользователь с таким именем уже существует, или если T это Person.
        /// В ином случае - сохраняет и возвращает новый экземпляр класса T.
        /// </returns>
        public static Person RegistrationOfPerson<T>(string nameOfPerson, string passwordOrSecondName, string pathToSave) where T : Person //TODO: Переделать комментарии к методам.
        {
            if (string.IsNullOrWhiteSpace(nameOfPerson)
            ||  string.IsNullOrWhiteSpace(passwordOrSecondName)
            ||  string.IsNullOrWhiteSpace(pathToSave))
            {
                return null;
            }
            var typeofT = typeof(T);
            try
            {
                using (var context = new UProduseStoreContext(pathToSave))
                {
                    if (typeofT == typeof(Client))
                    {
                        Client newUser = new Client(nameOfPerson, passwordOrSecondName);
                        Client userWithThisNameOrPassword = context.Clients
                                                                   .FirstOrDefault(client => client.Name == nameOfPerson
                                                                                          && client.Password == passwordOrSecondName);

                        if (newUser.Name != null && newUser.Password != null && userWithThisNameOrPassword == default)
                        {
                            context.Clients.Add(newUser);
                            context.SaveChanges();
                            return newUser;
                        }
                    }
                    else if (typeofT == typeof(Employee))
                    {
                        Employee newEmployee = new Employee(nameOfPerson, passwordOrSecondName);
                        Employee employeeWithThisNameOrPassword = context.Employees
                                                                         .FirstOrDefault(client => client.Name == nameOfPerson
                                                                                                && client.SecondName == passwordOrSecondName);
                        if (newEmployee.Name != null && newEmployee.SecondName != null && employeeWithThisNameOrPassword == default)
                        {
                            context.Employees.Add(newEmployee);
                            context.SaveChanges();
                            return newEmployee;
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
        public static void Registration_OfFullPerson<T>(Person person, string pathToSave) where T : Person
        {
            var typeofT = typeof(T);
            using (var context = new UProduseStoreContext(pathToSave))
            {
                if (typeofT == typeof(Client))
                {
                    Client newUser = person as Client;
                    context.Clients.Add(newUser);
                }
                else if (typeofT == typeof(Client))
                {
                    Employee newEmployee = person as Employee;
                    context.Employees.Add(newEmployee);
                }
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Увеличивает баланс экземпляра класса Client и сохраняет изменения клиента.
        /// </summary>
        /// <param name="client">Экземпляр класса Client, для которого будет произведено пополнение. </param>
        /// <param name="money">Сумма пополнения. </param>
        /// <returns> Возвращает true, если client не равен null, money более нуля и пополнение было успешно совершено.
        ///           Иначе - возвращает false. </returns>
        public static bool UpBalance(Client client, decimal money, string pathToSave) 
        {
            //Да, да, тут должна быть переадресация на платёжные системы и т.д.
            if (money > 0 && client != null)
            {
                client.Balance += money;
                SavePerson(client, pathToSave);
                return true;
            }
            else
            {
                Console.WriteLine("Сумма пополнения менее или равна нулю или аккаунт повреждён!");
                return false;
            }
        }

        /// <summary>
        /// Приватный метод, нужный для сохранения данных после операций ClientController.
        /// </summary>
        /// <param name="inputPerson">Сохраняемый клиент. </param>
        /// <param name="pathToSave">Путь к таблице/БД в которую должен быть сохранён экземпляр Client/Employee. </param>
        private static bool SavePerson(Person inputPerson, string pathToSave) 
        {
            if (inputPerson != null)
            {
                try
                {
                    using (var context = new UProduseStoreContext(pathToSave))
                    {
                        if (inputPerson is Client)
                        {
                            Client clientToSave = inputPerson as Client;

                            Client clientFromDB = context.Clients.FirstOrDefault(client => client.Name == clientToSave.Name
                                                                                        && client.Password == clientToSave.Password);
                            if (clientFromDB != null)
                            {
                                clientFromDB.Balance = clientToSave.Balance;  /*TODO: Бьюсь об заклад, это можно сделать лучше. 
                                                                                      Полное присоединение не работает. */
                                clientFromDB.Name = clientToSave.Name;
                                clientFromDB.Password = clientToSave.Password;
                                clientFromDB.Id = clientToSave.Id;

                                context.SaveChanges();
                                return true;
                            }
                        }
                        else if (inputPerson is Employee)
                        {
                            Employee employeeToSave = inputPerson as Employee;

                            Employee employeeFromDB = context.Employees.FirstOrDefault(employee => employee.Name == employeeToSave.Name
                                                                                                && employee.SecondName == employee.SecondName);
                            if (employeeFromDB != null)
                            {
                                employeeFromDB.Id = employeeToSave.Id;
                                employeeToSave.Name = employeeToSave.Name;
                                employeeToSave.Position = employeeToSave.Position;
                                employeeFromDB.Salary = employeeToSave.Salary;
                                employeeToSave.SecondName = employeeToSave.SecondName;

                                context.SaveChanges();
                                return true;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Thread.Sleep(5000);
                }
            }
            return false;
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
            if (client != null && string.IsNullOrWhiteSpace(nameOfTheProductBeDeleted) == false)
            {
                var bFormatter = new BinaryFormatter();
                string Path = SavePathParse(client);
                List<Product> listWithProducts = new List<Product>();

                using (var stream = new FileStream(Path, FileMode.OpenOrCreate))
                {
                    if (stream.Length != 0)
                    {
                        listWithProducts = bFormatter.Deserialize(stream) as List<Product>;
                    }
                }
                listWithProducts.RemoveAll(prod => prod.Name == nameOfTheProductBeDeleted);
                File.Delete(Path);

                using (var stream = new FileStream(Path, FileMode.Create))
                {
                    bFormatter.Serialize(stream, listWithProducts);
                }
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

        /// <summary>
        /// Строка подключения к БД сотрудников.
        /// </summary>
        public const string ConnectToMainServer = "MainServerConnection";

        /// <summary>
        /// Строка подключения к тестовой БД сотрудников.
        /// </summary>
        public const string ConnectToTestServer = "TestServerConnection";

        /// <summary>
        /// При использовании заменить слово NAME на другое значение.
        /// </summary>
        public const string SavePath = "BusketOf_NAME.dat";
        private static string SavePathParse(Client client)
        {
            if (client != null)
            {
                return SavePath.Replace("NAME", client.Name);
            }
            return "";
        }
    }
}

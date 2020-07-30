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
        /// Дополняет сериализованный список Product в файле пользователя. Если он не был создан - создаёт его.
        /// </summary>
        /// <param name="client">Клиент в "файл-корзину" которого будет добавлен продукт.</param>
        /// <param name="product">Объект Product, который будет добавлен.</param>
        /// <returns>   Возвращает true, если операции добавления и сериализации были успешно завершены.
        ///             Иначе, если client или product равны null и операции не были совершены, возвращает false.
        /// </returns>
        public static bool WriteProductInFileBasket(Client client, Product product)
        {
            if (client != null && product != null)
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
                    basket_With_Products.Add(product);
                    bFormatter.Serialize(stream, basket_With_Products);
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Дополняет сериализованный список Product в файле пользователя. Если он не был создан - создаёт его.
        /// Внимание, данный метод должен эксплуатироваться только в тестах!
        /// </summary>
        /// <param name="client">Клиент в "файл-корзину" которого будет добавлен продукт.</param>
        /// <param name="listOfProducts">Объекты Product, которые должны быть помещены в файл.</param>
        /// <returns>   Возвращает true, если операции добавления и сериализации были успешно завершены.
        ///             Иначе, если client или listOfProducts равны null и операции не были совершены, возвращает false.
        /// </returns>
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

        /// <summary>
        /// Считывает объекты Product из "файла-корзины" пользователя.
        /// </summary>
        /// <param name="client">Клиент, на основе данных которого будет создан путь к файлу.</param>
        /// <param name="thisListWillBeWrite">True - будет выведен на экран консоли, False - не будет.</param>
        /// <returns>
        /// Возвращает List объектов Product, если client не равен null и файл-корзина не пуст. Иначе - null.
        /// </returns>
        public static List<Product> ReadFileWithListOfProduct(Client client, bool thisListWillBeWrite)
        {
            if (client != null)
            {
                var bFormatter = new BinaryFormatter();
                string path = SavePathParse(client);

                using (var stream = new FileStream(path, FileMode.OpenOrCreate))
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
        /// Производит операцию покупки, считывая с счёта входного Client суммарную стоимость продуктов в его "файле-корзине",
        /// после чего удаляет её и сохраняет изменения.
        /// </summary>
        /// <param name="client">   Объект Client, на основе которого будет построен путь к файлу,
        ///                         и с которым будут проводится операции считывания со счёта. </param>
        ///
        /// <param name="context">  Объект UProduseStoreContext, на основе которого 
        ///                         будет создано подключение к серверу, нужное для сохранения изменений. </param>
        ///
        /// <param name="isUsed_InTest"> Параметр-флаг, если true - не отобразится информация о продуктах 
        ///
        ///                              в корзине пользователя и не будет требования подтверждения операции. 
        ///                              Не использовать нигде, кроме тестов! </param>
        ///
        /// <returns> Возвращает true, если Client и context не равны null, баланс клиента не менее или равен нулю и все операции были пройдены.
        ///           В ином случае, в т.ч при возникновении исключения - false. </returns>
        public static bool Buy(Client client, UProduseStoreContext context, bool isUsed_InTest)
        {
            if (client == null || context == null || client.Balance <= 0)
            {
                return false;
            }

            List<Product> listWithProducts = new List<Product>();
            string pathToBasket = SavePathParse(client);
            Client loadedClient = default;
            try
            {
                loadedClient = context.Clients.FirstOrDefault(clientFromDB => clientFromDB.Name == client.Name);

                using (var stream = new FileStream(pathToBasket, FileMode.OpenOrCreate))
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
                            File.Delete(pathToBasket);
                            loadedClient.Balance -= sumCostOfBusket;
                            SavePerson(loadedClient, context);
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
        /// Проверяет наличие объекта Client в базе данных на основе вводимых и возвращает его.
        /// </summary>
        /// <typeparam name="T">Тип проверяемого объекта. Ограничен Person и его наследниками.</typeparam>
        /// <param name="inputName">Имя проверяемого объекта.</param>
        /// <param name="passwordOrID">Пароль проверяемого объекта.</param>
        /// <param name="context">Объект подключения, на основе которого будет подключение к серверу.</param>
        /// <returns> Возвращает объект T, если он является Person. Может вернуть default/null, если он не был найден, 
        ///                                                                                     тип T указан как Person
        ///                                                                                     или если произошло исключение во время выполнения.
        /// </returns>
        public static Person LoadOfPerson<T>(string inputName, string passwordOrID, UProduseStoreContext context) where T : Person //TODO: ДОПИЛИТЬ ОПИСАНИЯ!!11
        {
            var nameOfType = typeof(T);
            try
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

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Сохраняет данные о пользователе в базу данных и возвращает его.
        /// </summary>
        /// <typeparam name="T"> Тип возвращаемого и сохраняемого значения. Ограничен Person и его потомками. </typeparam>
        /// <param name="nameOfPerson"> Имя нового пользователя. </param>
        /// <param name="passwordOrSecondName"> Пароль или фамилия нового пользователя.</param>
        /// <param name="context"> Объект подключения требуемый для, собственно, подключения к БД. </param>
        /// <returns>
        /// Возвращает null, если входные данные являются null, представляют пустую строку или если T = Person.
        /// В ином случае - сохраняет и возвращает новый экземпляр класса T.
        /// </returns>
        public static Person RegistrationOfPerson<T>(string nameOfPerson, string passwordOrSecondName, UProduseStoreContext context) where T : Person //TODO: Переделать комментарии к методам.
        {
            if (string.IsNullOrWhiteSpace(nameOfPerson)
            || string.IsNullOrWhiteSpace(passwordOrSecondName)
            || context == null)
            {
                return null;
            }
            var typeofT = typeof(T);
            try
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
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Сохраняет данные о пользователе в базу данных. Не использовать нигде, кроме тестов.
        /// </summary>
        /// <typeparam name="T">Тип сохраняемого значения. Ограничен Person и его потомками. </typeparam>
        /// <param name="person"> Полноценный экземпляр Person, который будет сохранён.  </param>
        /// <param name="context"> Объект подключения требуемый для, собственно, подключения к БД. </param>
        public static void Registration_OfFullPerson<T>(Person person, UProduseStoreContext context) where T : Person
        {
            var typeofT = typeof(T);

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

        /// <summary>
        /// Увеличивает значение Balance экземпляра Client и сохраняет эти изменения.
        /// </summary>
        /// <param name="client"> Экземпляр класса Client, для которого будет произведено пополнение. </param>
        /// <param name="money"> Сумма пополнения. </param>
        /// <param name="context"> Объект подключения требуемый для, собственно, подключения к БД. </param>
        /// <returns> Возвращает true, если client и context не равны null, money более нуля и пополнение было успешно совершено.
        ///           Иначе - возвращает false, предварительно выводя на экран сообщение об ошибке. </returns>
        public static bool UpBalance(Client client, decimal money, UProduseStoreContext context) 
        {
            //Да, да, тут должна быть переадресация на платёжные системы и т.д.
            if (money > 0 && client != null && context != null)
            {
                client.Balance += money;
                SavePerson(client, context);
                return true;
            }
            else
            {
                Console.WriteLine("Сумма пополнения менее или равна нулю или аккаунт повреждён!");
                return false;
            }
        }

        /// <summary>
        /// Приватный метод, нужный для сохранения данных после операций ClientController-а.
        /// </summary>
        /// <param name="inputPerson"> Сохраняемый наследник класса Person. </param>
        /// <param name="context"> Объект подключения требуемый для, собственно, подключения к БД. </param>
        /// <returns> Возвращает true, если client и context не равны null, и операция сохранения была успешно совершена.
        ///           Иначе - возвращает false, предварительно выводя на экран сообщение об ошибке. </returns>
        private static bool SavePerson(Person inputPerson, UProduseStoreContext context) 
        {
            if (inputPerson != null && context != null)
            {
                try
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
                        }
                    }
                    else
                    {
                        throw new ArgumentException($"Тип {inputPerson} не совпадает с требуемыми типами классов.");
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Thread.Sleep(5000);
                    return false;
                }
            }
            return false;
        }

        /// <summary>
        /// Удаляет экземпляр класса Product из файла-корзины, находимого на основе данных входного Client.
        /// В процессе данные из файла сохраняются и он пересоздаётся.
        /// </summary>
        /// <param name="client"> Экземпляр класса Client, из файла-корзины которого будет удалён экземпляр Product. </param>
        /// <param name="nameOfTheProductBeDeleted"> Имя удаляемого продукта. </param>
        /// <returns> Если client и имя удаляемого продукта не пусты и не равны пустой строке,и удаление прошло успешно - возвращает true.
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
                    if (stream.Length > 0)
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
        /// Возвращает пароль входного объекта Client. 
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
        /// Строка подключения к "боевой" БД.
        /// </summary>
        public const string ConnectToMainServer = "MainServerConnection";

        /// <summary>
        /// Строка подключения к тестовой БД.
        /// </summary>
        public const string ConnectToTestServer = "TestServerConnection";

        /// <summary>
        /// Константа - путь к файлу-корзине объекта Client. 
        /// При использовании заменить слово NAME на другое значение с помощью метода-парсера.
        /// </summary>
        public const string SavePath = "BusketOf_NAME.dat";

        /// <summary>
        /// Метод-парсер для константы, закреплённой как путь к файлу.
        /// </summary>
        /// <param name="client"> Объект Client, поле Name которого будет использовано для парса строки.</param>
        /// <returns> Возвращает это константу, изменённую под нужды метода. </returns>
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

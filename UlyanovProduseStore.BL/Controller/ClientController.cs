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
        //public static bool AddProductInBasket(Client client, Product product)
        //{
        //    if (product != null && client != null)
        //    {
        //        client.BasketOfproducts.Add(product);
        //        SaveClient(client);
        //        return true;
        //    }
        //    return false;
        //}

        //public static bool Buy(Client client)
        //{
        //    //TODO: Добавить проверку на наличие продуктов в файле/БД.
        //    if (client != null && client.BasketOfproducts.Count > 0)
        //    {
        //        var SumCost = client.BasketOfproducts.Select(x => x.Cost)
        //                                             .Sum();

        //        SumCost = SumCost * (decimal)client.DiscountRate;

        //        if (client.Balance >= SumCost)
        //        {
        //            client.Balance -= SumCost;
        //            client.DiscountRate -= (double)SumCost / 100000;
        //            client.DiscountRate = Math.Round(client.DiscountRate, 2); //TODO: Некорректное округление, исправить.

        //            if (client.DiscountRate < 0.90)
        //            {
        //                client.DiscountRate = 0.90;
        //            }
        //            client.BasketOfproducts = new List<Product>();
        //            SaveClient(client);
        //            return true;
        //        }
        //    }
        //    return false;
        //}

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

        public static Person RegistrationOfPerson<T>(string nameOfPerson, string passwordOrSecondName, string position, string pathToSave)
        {
            if (string.IsNullOrWhiteSpace(nameOfPerson) || string.IsNullOrWhiteSpace(passwordOrSecondName))
            {
                return null;
            }
            try
            {
                var nameOfPersonType = typeof(T);
                if (nameOfPersonType == typeof(Client))
                {
                    using (var context = new UPSClientContext(pathToSave))
                    {
                        var newClient = new Client(nameOfPerson, passwordOrSecondName);
                        if (newClient.Name != null && newClient.Password != null)
                        {
                            context.Clients.Add(newClient);
                            context.SaveChanges();
                            return newClient;
                        }
                    }
                }
                else if (nameOfPersonType == typeof(Employee))
                {
                    using (var context = new UPSEmployeeContext(pathToSave))
                    {
                        var newEmployee = new Employee(nameOfPerson, passwordOrSecondName, position);
                        if (newEmployee.Name != null && newEmployee.SecondName != null && newEmployee.Position != null)
                        {
                            context.Employees.Add(newEmployee);
                            context.SaveChanges();
                            return newEmployee;
                        }
                    }
                }
                throw new ArgumentException("Недопустимое значение Т!");
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
        public static bool UpBalance(Client client, decimal money, string pathToSave)
        {
            //Да, да, тут должна быть переадресация на платёжные системы и т.д.
            if (money > 0 && client != null)
            {
                client.Balance += money;
                SaveClient(client, pathToSave);
                return true;
            }
            else
            {
                Console.WriteLine("Сумма пополнения менее или равна нулю или аккаунт повреждён!");
                return false;
            }
        }

        
        private static void SaveClient(Client inputClient, string pathToSave)
        {
            try
            {
                using (var context = new UPSClientContext(pathToSave))
                {
                    var outClient = context.Clients.FirstOrDefault(client => client.Id == inputClient.Id);
                    outClient = inputClient;
                    context.SaveChanges();
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Не удалось сохранить данные после операции!");
                Thread.Sleep(5000);
            }
        }

        //public static bool DeleteProductFromBasket(Client client, string nameOfTheProductBeDeleted)
        //{
        //    if (client != null && client.BasketOfproducts.Count > 0 &&
        //        client.BasketOfproducts.Find(x => x.Name == nameOfTheProductBeDeleted) != default)
        //    {
        //        client.BasketOfproducts.RemoveAt(client.BasketOfproducts.FindIndex(x => x.Name == nameOfTheProductBeDeleted));

        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

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

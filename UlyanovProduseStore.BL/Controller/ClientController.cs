using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UlyanovProduseStore.BL.Model;

namespace UlyanovProduseStore.BL.Controller
{
    public class ClientController
    {
        /// <summary>
        /// Добавляет в поле клиента "BasketOfproducts" экземпляр класса Product.
        /// </summary>
        /// <param name="client">Клиент в "корзину" которого будет добавлен продукт.</param>
        /// <param name="product">Экземпляр класса Product.</param>
        public static void AddProductInBasket(Client client, Product product)
        {
            if (product != null && client != null)
            {
                client.BasketOfproducts.Add(product);
            }
            else
            {
                throw new ArgumentNullException("Покупатель и/или продукт являются пустыми объектами!");
            }
        }

        /// <summary>
        /// Считывает с счёта клиента стоимость всех продуктов в его корзине.
        /// </summary>
        /// <param name="client">Объект класса Client. </param>
        /// <returns> True - если счёт клиента больше или равен сумме стоимости продуктов и была проведена операция вычитания. 
        ///           False - если счёт клиента меньше суммы стоимости продуктов или клиент пуст или корзина пуста. </returns>
        public static bool Buy(Client client)
        {
            if (client != null && client.BasketOfproducts.Count > 0)
            {
                var SumCost = client.BasketOfproducts.Select(x => x.Cost).Sum();

                SumCost = SumCost * (decimal)client.DiscountRate;

                if (client.Account >= SumCost)
                {
                    client.Account -= SumCost;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Метод поиска данных о пользователе среди сериализованных файлов. Открывает файл с данными по пути указанному 
        /// в NAME_TO_USERDATA (или если там пусто - создаёт пустой файл) и проверяет на заполненность.
        /// Если заполнен - заполняет аргументный person данными из файла. 
        /// Иначе - если данные клиента - сериализует их. 
        /// </summary>
        /// <param name="person">Новый(только имя) клиент.</param>
        /// <returns> Возвращает true, если данные удалось десериализовать и имя сохранённого клиента совпадает с входным, 
        ///           и возвращает false, если файл не найден/пуст/имя не совпадает. В таком случае, сериализует клиента в него. </returns>
        public static bool FindPerson<T>(T person) where T: Person //TODO: Доделать описание.
        {
            var binFormatter = new BinaryFormatter();
            try
            {
                using (var stream = new FileStream(person.GetPathToUserData(), FileMode.OpenOrCreate))
                {
                    if (stream.Length == 0)
                    {
                        binFormatter.Serialize(stream, person);
                        Console.WriteLine("Вы не зарегистрированы и ранее не заходили, данные о вас сгенерированы и сохранены по умолчанию.");
                    }
                    else if (typeof(T) == typeof(Client))
                    {
                        person = binFormatter.Deserialize(stream) as T;
                        return true;
                    }
                    else if(typeof(T) == typeof(Employee))
                    {
                        return true;
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("Не удалось получить данные о вас или файл о вас повреждён!");
            }
        }

        /// <summary>
        /// Увеличивает баланс экземпляра класса Client.
        /// </summary>
        /// <param name="client">Экземпляр класса Client.</param>
        /// <param name="money">Сумма для пополнения.</param>
        public static bool UpBalance(Client client, decimal money)
        {
            //Да, да, тут должна быть переадресация на платёжные системы и т.д.
            if (money > 0 && client != null)
            {
                client.Account += money;
                return true;
            }
            else
            {
                Console.WriteLine("Сумма пополнения менее или равна нулю или аккаунт повреждён!");
                return false;
            }
        }
    }
}

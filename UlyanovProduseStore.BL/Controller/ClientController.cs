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
        /// Метод поиска данных о клиенте среди сериализованных файлов. Открывает файл с данными (или если он пуст - создаёт) и проверяет
        /// на заполненность. Если заполнен - заполняет аргументный client данными из файла. Иначе - сериализует его данные.
        /// </summary>
        /// <param name="clientFromFIle">Новый(только имя) клиент.</param>
        /// <returns> Возвращает true, если данные удалось десериализовать и имя сохранённого клиента совпадает с входным, 
        ///           и возвращает false, если файл не найден/пуст/имя не совпадает. В таком случае, сериализует клиента в него. </returns>
        public static bool FindClient(Client client)
        {
            bool IsClientFind = false;
            var binFormatter = new BinaryFormatter();

            try
            {
                using (var stream = new FileStream(client.NAME_TO_USERDATA, FileMode.OpenOrCreate))
                {
                    if (stream.Length > 0)
                    {
                        client = binFormatter.Deserialize(stream) as Client;
                        IsClientFind = true;
                    }      
                    else
                    {
                        binFormatter.Serialize(stream, client);
                    }
                }
            }
            catch (Exception)
            {
                throw new Exception("Не удалось получить данные о клиенте или файл о нём повреждён!");
            }

            return IsClientFind;
        }
        /// <summary>
        /// Увеличивает баланс пользователя.
        /// </summary>
        /// <param name="client">Экземпляр класса Client.</param>
        /// <param name="money">Количество денег для пополнения.</param>
        public static void UpBalance(Client client, decimal money)
        {
            //Да, да, тут должна быть переадресация на платёжные системы и т.д.
            if (money > 0)
            {
                client.Account += money;
            }
            else
            {
                throw new ArgumentException("Сумма пополнения должна быть более нуля!");
            }
        }
    }
}

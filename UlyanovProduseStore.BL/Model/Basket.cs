using System.Collections.Generic;
using System.Linq;

namespace UlyanovProduseStore.BL.Model
{
    /// <summary>
    /// Виртуальная сущность корзины, содержащая экземпляры Product и "владельца" этого объекта Basket - экземпляра Client. 
    /// </summary>
    public class Basket
    {
        /// <summary>
        /// Создаёт новый экземпляр Basket.
        /// </summary>
        /// <param name="client"> "Владелец" данного экземпляра Basket. </param>
        public Basket(Client client)
        {
            if (client != null)
            {
                Client = client;
            }
        }

        /// <summary>
        /// Создаёт новый экземпляр Basket.
        /// </summary>
        /// <param name="client"> "Владелец" данного экземпляра Basket. </param>
        /// <param name="product"> Первый объект Product, который будет сдержатся в этом экземпляре Basket. </param>
        public Basket(Client client, Product product)
        {
            if (client != null && product != null)
            {
                Client = client;
                Products.Add(product);
            }
        }

        /// <summary>
        /// Создаёт новый экземпляр Basket.
        /// </summary>
        /// <param name="client"> "Владелец" данного экземпляра Basket. </param>
        /// <param name="products"> Объекты Product, которые должны быть добавлены. </param>
        public Basket(Client client, List<Product> products)
        {
            if (client != null && products != null)
            {
                Client = client;
                Products.AddRange(products);
            }
        }

        #region params

        /// <summary>
        /// "Владелец" данного экземпляра Basket.
        /// </summary>
        public Client Client { get; set; }

        /// <summary>
        /// Объекты Product, хранящиеся в этом экземпляре Basket.
        /// </summary>
        public List<Product> Products = new List<Product>();

        #endregion

        /// <summary>
        /// Возвращает суммарную стоимость всех объектов Product в этом экземпляре Basket.
        /// </summary>
        /// <returns></returns>
        public decimal SumCostOfThisBasket()
        {
            return Products.Sum(prod => prod.Cost);
        }
    }
}

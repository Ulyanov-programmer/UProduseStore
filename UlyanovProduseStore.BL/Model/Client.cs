using System;
using System.Collections.Generic;

namespace UlyanovProduseStore.BL.Model
{
    [Serializable]
    public class Client
    {
        internal string NAME_TO_USERDATA;

        public Client(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name), "Имя не может быть пустым!");
            }

            Name = name;
            BasketOfproducts = new List<Product>();
            Account = 0;
            DiscountRate = 1.00F;
            NAME_TO_USERDATA = $"user{Name}.dat"; //TODO: Это не безопасно, хоть и очень удобно.
        }

        /// <summary>
        /// Конструктор для тестов, позволяет присвоить значения всем полям.
        /// </summary>
        /// <param name="name">имя.</param>
        /// <param name="products">Корзина(лист) продуктов.</param>
        /// <param name="discountRate">Коэффициент скидки в виде "Х.ХХF".</param>
        /// <param name="account">Баланс.</param>
        public Client(string name, List<Product> products, float discountRate, decimal account)
        {
            Name = name;
            DiscountRate = discountRate;
            Account = account;
            BasketOfproducts.AddRange(products);
            NAME_TO_USERDATA = $"user{Name}.dat";
        }

        #region params

        internal string Name { get; set; }

        internal List<Product> BasketOfproducts = new List<Product>();

        /// <summary>
        /// Указывает коэффициент скидки в формате "Х.ХХF". Не может быть менее "0.90". 
        /// </summary>
        internal float DiscountRate { get; set; }
        internal decimal Account { get; set; }
        #endregion
    }
}

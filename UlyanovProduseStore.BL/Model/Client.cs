using System;
using System.Collections.Generic;

namespace UlyanovProduseStore.BL.Model
{
    /// <summary>
    /// Класс клиента. Содержит его корзину (лист продуктов), коэффициент скидки в формате "X.XX" (не более единицы) и баланс.
    /// </summary>
    [Serializable]
    public class Client : Person
    {
        /// <summary>
        /// Создаёт новый (полупустой) экземпляр Client с заполненным именем. Остальные параметры выставляются по умолчанию.
        /// </summary>
        /// <param name="name"> Имя "пустого" клиента.</param>
        /// <param name="typeOfAssembly"> Путь к сборке. </param>
        public Client(string name) : base(name)
        {
            BasketOfproducts = new List<Product>();
            Balance = 0;
            DiscountRate = 1.00F;
        }

        /// <summary>
        /// Конструктор для тестов, позволяет присвоить значения всем полям.
        /// </summary>
        /// <param name="name">Имя пользователя.</param>
        /// <param name="products">Корзина(лист) продуктов.</param>
        /// <param name="discountRate">Коэффициент скидки в виде "Х.ХХF".</param>
        /// <param name="balance">Баланс.</param>
        /// <param name="typeOfAssembly"> Путь к сборке.  </param>
        public Client(string name, List<Product> products, float discountRate, decimal balance) : base(name)
        {
            #region Cheks

            if (discountRate > 1.00 || discountRate < 0.90)
            {
                throw new ArgumentException("Коэффициент скидки не может быть более 1.00 или менее 0.90!", nameof(discountRate));
            }
            if (products == null || products.Count == 0)
            {
                throw new ArgumentException("Лист продуктов не может быть null или равен нулю!", nameof(products));
            }
            if (balance <= 0)
            {
                throw new ArgumentException("Баланс не может быть менее или равен нулю!", nameof(balance));
            }
            #endregion

            DiscountRate = discountRate;
            Balance = balance;
            BasketOfproducts.AddRange(products);
        }

        #region params

        internal List<Product> BasketOfproducts = new List<Product>();
        /// <summary>
        /// Указывает коэффициент скидки в формате "Х.ХХ". Не может быть менее "0.90". 
        /// </summary>
        internal double DiscountRate { get; set; }
        internal decimal Balance { get; set; }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace UlyanovProduseStore.BL.Model
{
    [DataContract]
    public class Product
    {
        internal const string PathSaveOfProducts = @"F:\Projects\UlyanovProduseStore\UlyanovProduseStore.BL\bin\Debug\products.json";
        //TODO: При допилке на базу данных - удалить нахрен этот костыль.

        internal static List<string> Categories = new List<string>
        {"Cold_Drink", "Fruits" }; // Не ненадёжно, но IEnumerable неудобен (будет позже).

        /// <summary>
        /// Создаёт экземпляр класса Product.
        /// </summary>
        /// <param name="name"> Его имя.</param>
        /// <param name="cost"> Его стоимость ( на данный момент - в рублях). </param>
        /// <param name="numberOfCategory"> Его номер категории (на данный момент доступно две: 0 = Cold_Drink, 1 = Fruits).</param>
        public Product(string name, decimal cost, byte numberOfCategory)
        {
            #region Cheks

            if (string.IsNullOrWhiteSpace(name) || name.Contains("."))
            {
                throw new ArgumentNullException(nameof(name), "Имя не может быть пустым или содержать точки!");
            }

            if (cost <= 0)
            {
                throw new ArgumentNullException(nameof(cost), "Стоимость продукта не может быть ниже или равна нулю!");
            }

            if (numberOfCategory >= Categories.Count || numberOfCategory < 0)
            {
                throw new ArgumentOutOfRangeException("Номер категории не может быть таким!");
            }
            #endregion

            Name = name;
            Cost = cost;
            Category = Categories[numberOfCategory];
        }

        #region Params

        [DataMember]
        internal string Name { get; set; }
        [DataMember]
        internal decimal Cost { get; set; }
        [DataMember]
        internal string Category { get; set; }
        #endregion

        /// <summary>
        /// Возвращает Name экземпляра Product.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Name.ToString();
        }
    }
}

using System;
using System.Collections.Generic;

namespace UlyanovProduseStore.BL.Model
{
    [Serializable]
    public class Product
    {
        internal static List<string> Categories = new List<string>
        {"Холодные напитки", "Фрукты" }; // Не ненадёжно, но IEnumerable неудобен (будет позже). 
        //При допилке под другие языки следует вставить другие названия категорий. 

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

        /// <summary>
        /// Путь к месту сохранения продуктов.
        /// </summary>
        internal const string PathSaveOfProducts = @"F:\Projects\UlyanovProduseStore\UlyanovProduseStore.BL\bin\Debug\Data\products.dat";
        //TODO: При допилке на базу данных - удалить нахрен этот костыль.
        internal string Name { get; set; }
        internal decimal Cost { get; set; }
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

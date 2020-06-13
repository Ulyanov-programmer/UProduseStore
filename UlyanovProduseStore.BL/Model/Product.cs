using System;
using System.Collections.Generic;

namespace UlyanovProduseStore.BL.Model
{
    [Serializable]
    public class Product
    {
        internal static List<string> Categories = new List<string> 
        { "Fruits", "Vegetables", "Cold Drink" }; //TODO: заменить чем-то более функциональным.
        public Product(string name, decimal cost, byte numberOfCategory)
        {
            #region Cheks

            if (string.IsNullOrWhiteSpace(name) || name.Contains("."))
            {
                throw new ArgumentNullException(nameof(name), "Имя не может быть пустым!");
            }

            if (cost <= 0)
            {
                throw new ArgumentNullException(nameof(cost), "Стоимость продукта не может быть ниже или равна нулю!");
            }

            if (numberOfCategory > 4 || numberOfCategory < 0) //TODO: Исправить этот говнокод
            {
                throw new ArgumentException("Такой категории не существует!", nameof(numberOfCategory));
            }
            #endregion

            Name = name;
            Cost = cost;
            Category = Categories[numberOfCategory];
        }

        #region Params

        internal string Name { get; set; }
        internal decimal Cost { get; set; }
        internal string Category { get; set; }
        #endregion

    }
}

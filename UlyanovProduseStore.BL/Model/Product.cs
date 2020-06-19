using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace UlyanovProduseStore.BL.Model
{
    [DataContract]
    public class Product
    {
        internal const string PathSaveOfProducts = @"F:\Projects\UlyanovProduseStore\UlyanovProduseStore.BL\bin\Debug\products.json";
        //TODO: При создании установщика изменить тип указания пути к папке с Products.

        internal static List<string> Categories = new List<string>
        {"Cold_Drink", "Fruits" }; // Не ненадёжно, но enum нельзя перечислить а IEnumerable неудобен.


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

        public override string ToString()
        {
            return Name.ToString();
        }
    }
}

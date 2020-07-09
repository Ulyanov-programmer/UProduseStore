using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace UlyanovProduseStore.BL.Model
{
    [Serializable]
    public class Product
    {
        internal static List<string> Categories = new List<string>
        {"Холодные напитки", "Фрукты" };

        /// <summary>
        /// Создаёт экземпляр класса Product.
        /// </summary>
        /// <param name="name"> Его имя.</param>
        /// <param name="cost"> Его стоимость ( на данный момент - в рублях). </param>
        /// <param name="numberOfCategory"> Его номер категории (на данный момент доступно две: 0 = Cold_Drink, 1 = Fruits).</param>
        public Product(string name, decimal cost, int numberOfCategory)
        {
            StringBuilder messageAboutExeption = new StringBuilder();
            #region Cheks

            if (string.IsNullOrWhiteSpace(name) || name.Contains("."))
            {
                messageAboutExeption.AppendLine("Имя не может быть пустым или содержать точки!");
            }

            if (cost <= 0)
            {
                messageAboutExeption.AppendLine("Стоимость продукта не может быть ниже или равна нулю!");
            }

            if (numberOfCategory >= Categories.Count || numberOfCategory < 0)
            {
                messageAboutExeption.AppendLine("Номер категории не может быть таким!");
            }
            #endregion

            if (messageAboutExeption.Length > 0)
            {
                Console.WriteLine(messageAboutExeption);
                return; // Если вызвать return до присвоения значений, им будет установлено default.    
            }
            
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

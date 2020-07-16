using System;
using System.Text;

namespace UlyanovProduseStore.BL.Model
{
    public class Product
    {
        public Product(string name, decimal cost)
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
            #endregion

            if (messageAboutExeption.Length > 0)
            {
                Console.WriteLine(messageAboutExeption);
                return;   
            }
            
            Name = name;
            Cost = cost;
        }

        #region Params

        internal string Name { get; set; }
        internal decimal Cost { get; set; }
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

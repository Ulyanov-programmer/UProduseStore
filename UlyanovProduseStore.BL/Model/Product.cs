using System;
using System.Text;

namespace UlyanovProduseStore.BL.Model
{
    public class Product
    {
        public Product()
        {
        }

        public Product(string name, decimal cost)
        {
            var messageAboutExeption = new StringBuilder();
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

        #region params

        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Cost { get; set; }
        #endregion

        #region overrides

        /// <summary>
        /// Возвращает Name экземпляра Product.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Name;
        }

        #endregion
    }
}

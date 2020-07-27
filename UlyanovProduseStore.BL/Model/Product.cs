using System;
using System.Collections.Generic;
using System.Text;

namespace UlyanovProduseStore.BL.Model
{
    [Serializable]
    public class Product
    {
        /// <summary>
        /// Создаёт экземпляр класса Product.
        /// </summary>
        /// <param name="name"> Его имя.</param>
        /// <param name="cost"> Его стоимость ( на данный момент - в рублях). </param>
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
            //TODO: Добавить метод присвоения категории продукту.
            //if (numberOfCategory >= Categories.Count || numberOfCategory < 0)
            //{
            //    messageAboutExeption.AppendLine("Номер категории не может быть таким!");
            //}
            #endregion

            if (messageAboutExeption.Length > 0)
            {
                Console.WriteLine(messageAboutExeption);
                return;
            }

            Name = name;
            Cost = cost;
        }

        public Product() { }

        #region Params

        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Cost { get; set; }
        public string CategoryId { get; set; }
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

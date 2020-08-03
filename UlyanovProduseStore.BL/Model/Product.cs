using System;
using System.Text;

namespace UlyanovProduseStore.BL.Model
{
    /// <summary>
    /// Класс продукта, определяющий его основные поля, такие как имя и стоимость. Может быть сериализован и добавлен в таблицу в БД.
    /// </summary>
    [Serializable]
    public class Product
    {
        /// <summary>
        /// Создаёт экземпляр класса Product.
        /// </summary>
        /// <param name="name"> Его имя. </param>
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
            #endregion

            if (messageAboutExeption.Length > 0)
            {
                Console.WriteLine(messageAboutExeption);
                return;
            }

            Name = name;
            Cost = cost;
        }

        /// <summary>
        /// Пустой конструктор для работы Entity Framework.
        /// </summary>
        public Product() { }

        #region Params

        /// <summary>
        /// Идентификатор экземпляра Product согласно данным из БД.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Имя экземпляра Product.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Стоимость экземпляра Product.
        /// </summary>
        public decimal Cost { get; set; }

        /// <summary>
        /// Это вообще хрен его знает зачем тут.
        /// </summary>
        public string CategoryId { get; set; } //TODO: В планах добавить значение категории через связь таблиц.
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

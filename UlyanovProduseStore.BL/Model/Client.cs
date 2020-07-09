using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace UlyanovProduseStore.BL.Model
{
    /// <summary>
    /// Класс клиента. Содержит имя, пароль, его корзину (лист продуктов), коэффициент скидки в формате "X.XX" (не более единицы) и баланс.
    /// </summary>
    [Serializable]
    public class Client : Person
    {
        /// <summary>
        /// Создаёт новый экземпляр Client с заполненным именем и паролем. Остальные параметры выставляются по умолчанию.
        /// </summary>
        /// <param name="name"> Имя "пустого" клиента.</param>
        /// <param name="typeOfAssembly"> Путь к сборке. </param>
        public Client(string name, string password) : base(name)
        {
            StringBuilder stringAboutExeption = new StringBuilder();
            #region Cheks

            if (string.IsNullOrWhiteSpace(password) || password.Length < 5)
            {
                stringAboutExeption.AppendLine("Пароль не может быть пустым, состоять только из символов разделителей, или быть менее 5 символов!");
            }

            #endregion

            if (stringAboutExeption.Length > 0)
            {
                Console.WriteLine(stringAboutExeption);
                return;
            }

            Password = password;
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
        public Client(string name, string password, List<Product> products, double discountRate, decimal balance) : base(name)
        {
            StringBuilder stringAboutExeption = new StringBuilder();
            #region Cheks

            if (string.IsNullOrWhiteSpace(password))
            {
                stringAboutExeption.AppendLine("Пароль не может быть пустым, состоять только из пробелов или из символов разделителей!");
            }
            if (discountRate > 1.00 || discountRate < 0.90)
            {
                stringAboutExeption.AppendLine("Коэффициент скидки не может быть более 1.00 или менее 0.90!");
            }
            if (products == null)
            {
                stringAboutExeption.AppendLine("Лист продуктов не может быть null или равен нулю!");
            }
            if (balance <= 0)
            {
                stringAboutExeption.AppendLine("Баланс не может быть менее или равен нулю!");
            }
            #endregion

            if (stringAboutExeption.Length > 0)
            {
                Console.WriteLine(stringAboutExeption);
                return;
            }
            Password = password;
            DiscountRate = discountRate;
            Balance = balance;
            BasketOfproducts = new List<Product>();
            BasketOfproducts.AddRange(products);
        }

        #region params
        internal string Password;

        internal List<Product> BasketOfproducts;

        /// <summary>
        /// Указывает коэффициент скидки в формате "Х.ХХ". Не может быть менее "0.90". 
        /// </summary>
        internal double DiscountRate { get; set; }
        internal decimal Balance { get; set; }
        #endregion
    }
}

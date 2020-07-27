using System;
using System.Collections.Generic;
using System.Text;
using UlyanovProduseStore.BL.Controller;

namespace UlyanovProduseStore.BL.Model
{
    /// <summary>
    /// Класс клиента. Содержит имя, пароль, его корзину (лист продуктов), коэффициент скидки в формате "X.XX" (не более единицы) и баланс.
    /// </summary>
    public class Client : Person
    {
        /// <summary>
        /// Создаёт новый экземпляр Client с заполненным именем и паролем. Остальные параметры выставляются по умолчанию.
        /// </summary>
        /// <param name="name"> Имя "пустого" клиента.</param>
        /// <param name="password">Пароль нового пользователя.</param>
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
            Balance = 0;
        }

        /// <summary>
        /// Конструктор для тестов, позволяет присвоить значения всем полям.
        /// </summary>
        /// <param name="name">Имя пользователя.</param>
        /// <param name="products">Корзина(лист) продуктов.</param>
        /// <param name="discountRate">Коэффициент скидки в виде "Х.ХХF".</param>
        /// <param name="balance">Баланс.</param>
        /// <param name="password">Пароль нового пользователя.</param>
        public Client(string name, string password, List<Product> identifiersOfProducts, decimal balance) : base(name)
        {
            StringBuilder stringAboutExeption = new StringBuilder();
            #region Cheks

            if (string.IsNullOrWhiteSpace(password))
            {
                stringAboutExeption.AppendLine("Пароль не может быть пустым, состоять только из пробелов или из символов разделителей!");
            }
            if (identifiersOfProducts == null)
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
            Balance = balance;
            ClientController.WriteProductInFileBasket(this, identifiersOfProducts);
        }

        public Client() : base("X") { }

        #region params
        public int Id { get; set; }
        public string Password { get; set; }

        //public double DiscountRate { get; set; }
        public decimal Balance { get; set; } 
        #endregion
    }
}

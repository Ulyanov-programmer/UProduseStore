using System;
using System.Collections.Generic;
using System.Text;
using UlyanovProduseStore.BL.Controller;

namespace UlyanovProduseStore.BL.Model
{
    /// <summary>
    /// Класс клиента, нужный для соответствующих функций.
    /// </summary>
    public class Client : Person
    {
        /// <summary>
        /// Создаёт новый экземпляр Client с заполненным именем и паролем. Остальные параметры выставляются по умолчанию.
        /// </summary>
        /// <param name="name"> Имя "пустого" клиента. </param>
        /// <param name="password"> Пароль нового пользователя, не может быть менее 5 символов. </param>
        public Client(string name, string password) : base(name)
        {
            StringBuilder messageAboutExeption = new StringBuilder();
            #region Cheks

            if (string.IsNullOrWhiteSpace(password) || password.Length < 5)
            {
                messageAboutExeption.AppendLine("Пароль не может быть пустым, состоять только из символов разделителей, или быть менее 5 символов!");
            }
            #endregion

            if (messageAboutExeption.Length > 0)
            {
                Console.WriteLine(messageAboutExeption);
                return;
            }

            Password = password;
            Balance = 0;
        }

        /// <summary>
        /// Конструктор для тестов, позволяет присвоить значения всем полям, а так-же заполнить файл-корзину.
        /// </summary>
        /// <param name="name"> Новое имя пользователя. </param>
        /// <param name="identifiersOfProducts"> Корзина(лист) продуктов, которыми будет заполнен файл-корзина. </param>
        /// <param name="balance"> Новый баланс.</param>
        /// <param name="password"> Новый пароль. </param>
        public Client(string name, string password, List<Product> identifiersOfProducts, decimal balance) : base(name)
        {
            StringBuilder messageAboutExeption = new StringBuilder();
            #region Cheks

            if (string.IsNullOrWhiteSpace(password))
            {
                messageAboutExeption.AppendLine("Пароль не может быть пустым, состоять только из пробелов или из символов разделителей!");
            }
            if (identifiersOfProducts == null)
            {
                messageAboutExeption.AppendLine("Лист продуктов не может быть null или равен нулю!");
            }
            if (balance <= 0)
            {
                messageAboutExeption.AppendLine("Баланс не может быть менее или равен нулю!");
            }
            #endregion

            if (messageAboutExeption.Length > 0)
            {
                Console.WriteLine(messageAboutExeption);
                return;
            }
            Password = password;
            Balance = balance;
            ClientController.WriteProductInFileBasket(this, identifiersOfProducts);
        }

        /// <summary>
        /// Пустой конструктор, нужный для работы Entity Framework.
        /// </summary>
        public Client() : base("X") { }

        #region params

        /// <summary>
        /// Идентификатор пользователя согласно данным из БД.
        /// </summary>
        public int Id { get; set; } //TODO: Усилить защиту полей.

        /// <summary>
        /// Пароль пользователя.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Баланс пользователя (в валюте).
        /// </summary>
        public decimal Balance { get; set; } 
        #endregion
    }
}

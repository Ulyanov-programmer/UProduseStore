using System;

namespace UlyanovProduseStore.BL.Model
{
    /// <summary>
    /// Абстрактный класс для предотвращения дублирования кода у его наследников.
    /// </summary>
    public abstract class Person
    {
        /// <summary>
        /// Доступный только для наследников Person конструктор, в нём определяется значение общих полей.
        /// </summary>
        /// <param name="name">Имя наследника Person.</param>
        protected Person(string name, string password)
        {
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(password))
            {
                Console.WriteLine("Имя и/или пароль не могут быть пустыми или состоять только из символов - разделителей!");
                return;
            }

            Password = password;
            Name = name;
        }

        #region Params

        public string Name { get; set; }
        public string Password { get; set; }

        #endregion

        #region overrides

        /// <summary>
        /// Возвращает Name экземпляра производного от Person..
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Name;
        }

        #endregion
    }
}

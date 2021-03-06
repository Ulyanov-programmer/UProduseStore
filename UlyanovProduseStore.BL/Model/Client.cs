﻿namespace UlyanovProduseStore.BL.Model
{
    /// <summary>
    /// Содержит данные о клиенте, такие как его баланс, имя и т.п .
    /// </summary>
    public class Client : Person
    {
        public Client() : base("X", "X") { }

        /// <summary>
        /// Создаёт новый экземпляр Client.
        /// </summary>
        /// <param name="name"> Имя нового экземпляра Client. </param>
        /// <param name="password"> Пароль нового экземпляра Client. </param>
        public Client(string name, string password) : base(name, password)
        {
            Balance = 0;
        }

        /// <summary>
        /// Создаёт новый экземпляр Client.
        /// </summary>
        /// <param name="name"> Имя нового экземпляра Client. </param>
        /// <param name="password"> Пароль нового экземпляра Client. </param>
        /// <param name="balance"> Баланс нового экземпляра Client. </param>
        public Client(string name, string password, decimal balance) : base(name, password)
        {
            if (balance > 0)
            {
                Balance = balance;
            }
        }

        #region params

        public int Id { get; set; }
        public decimal Balance { get; set; }

        #endregion

        #region overrides

        /// <summary>
        /// Сравнивает два экземпляра Client.
        /// </summary>
        /// <param name="otherClient"> Другой экземпляр Client. </param>
        /// <returns> True если сравниваемые объекты равны по определённым параметрам, иначе - false. </returns>
        public bool Equals(Client otherClient)
        {
            if (otherClient.Name == Name &&
                otherClient.PasswordOrSecondName == PasswordOrSecondName)
            {
                return true;
            }
            return false;
        }

        #endregion
    }
}

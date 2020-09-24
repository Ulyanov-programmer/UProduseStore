using System;
using System.Text;

namespace UlyanovProduseStore.BL.Model
{
    public class Client : Person
    {
        public Client() : base("X", "X")
        {
        }

        public Client(string name, string password) : base(name, password)
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

            Balance = 0;
        }

        public Client(string name, string password, decimal balance) : base(name, password)
        {
            StringBuilder stringAboutExeption = new StringBuilder();
            #region Cheks

            if (string.IsNullOrWhiteSpace(password))
            {
                stringAboutExeption.AppendLine("Пароль не может быть пустым, состоять только из символов разделителей!");
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

            Balance = balance;
        }

        #region params

        public int Id { get; set; }
        public decimal Balance { get; set; }

        #endregion
    }
}

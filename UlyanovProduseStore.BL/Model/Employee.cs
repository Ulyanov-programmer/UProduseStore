using System;
using System.Text;

namespace UlyanovProduseStore.BL.Model
{
    /// <summary>
    /// Класс сотрудника компании, с его помощью можно получить доступ к особым функциям контроллера.
    /// </summary>
    public class Employee : Person 
    {
        /// <summary>
        /// Создаёт новый экземпляр класса Employee.
        /// </summary>
        /// <param name="name"> Его имя. </param>
        /// <param name="secondName"> Его фамилия. </param>
        public Employee(string name, string secondName) : base(name)
        {
            StringBuilder messageAboutExeption = new StringBuilder();
            #region cheks

            if (string.IsNullOrWhiteSpace(secondName))
            {
                messageAboutExeption.AppendLine("Фамилия не может быть пустой!");
                return;
            }
            #endregion

            if (messageAboutExeption.Length > 0)
            {
                Console.WriteLine($"Произошла одна или несколько ошибок: {messageAboutExeption}");
                return;
            }
            SecondName = secondName;
        }

        /// <summary>
        /// Пустой конструктор, необходимый для работы Entity Framework.
        /// </summary>
        public Employee() : base("X") { }


        #region params

        /// <summary>
        /// Идентификатор экземпляра Employee, согласно данным из БД.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Фамилия экземпляра Employee.
        /// </summary>
        public string SecondName { get; set; }

        /// <summary>
        /// Должность экземпляра Employee. Может быть Null.
        /// </summary>
        public string Position { get; set; }

        /// <summary>
        /// Зарплата экземпляра Employee. Может быть 0, поле заполнять напрямую из БД.
        /// </summary>
        public decimal Salary { get; set; }  
        #endregion
    }
}

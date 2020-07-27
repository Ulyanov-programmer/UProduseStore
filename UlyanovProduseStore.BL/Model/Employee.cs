using System;
using System.Text;

namespace UlyanovProduseStore.BL.Model
{
    public class Employee : Person //TODO: После допилки под БД, дополнить описание.
    {
        /// <summary>
        /// Создаёт новый экземпляр класса Employee.
        /// </summary>
        /// <param name="name"> Его имя. </param>
        public Employee(string name, string secondName) : base(name)
        {
            StringBuilder stringAboutExeption = new StringBuilder();
            if (string.IsNullOrWhiteSpace(secondName))
            {
                stringAboutExeption.AppendLine("Фамилия не может быть пустой!");
                return;
            }
            SecondName = secondName;
        }
        public Employee() : base("X") { }


        #region params

        public int Id { get; set; } 
        public string SecondName { get; set; }
        public string Position { get; set; } 
        public decimal Salary { get; set; }  
        #endregion
    }
}

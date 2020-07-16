using System;
using System.Text;

namespace UlyanovProduseStore.BL.Model
{
    public class Employee : Person //TODO: После допилки под БД, дополнить описание.
    {
        public Employee(string name, string secondName, string position) : base(name)
        {
            StringBuilder stringAboutExeption = new StringBuilder();
            #region cheks

            if (string.IsNullOrWhiteSpace(secondName))
            {
                stringAboutExeption.AppendLine("Фамилия не может быть пустой!");
            }
            if (string.IsNullOrWhiteSpace(position))
            {
                stringAboutExeption.AppendLine("Должность не может быть пустой!");
            }
            #endregion

            SecondName = secondName;
            Position = position;
        }

        #region params

        public int Id { get; set; }
        public string SecondName { get; set; }
        public string Position { get; set; }
        public decimal Salary { get; set; }
        #endregion
    }
}

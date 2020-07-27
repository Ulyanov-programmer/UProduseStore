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
        public Person(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Имя не может быть пустым или состоять только из символов - разделителей!");
                return;
            }

            Name = name;
        }

        #region params

        public string Name { get; set; }
        #endregion

        /// <summary>
        /// Возвращает поле Name экземпляра класса, наследника от Person. 
        /// </summary>
        /// <returns> </returns>
        public override string ToString()
        {
            return Name;
        }
    }
}

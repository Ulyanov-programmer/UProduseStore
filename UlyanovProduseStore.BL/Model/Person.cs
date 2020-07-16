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
        protected Person(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Имя не может быть пустым или состоять только из символов - разделителей!");
                return;
            }

            Name = name;
        }

        #region Params

        /// <summary>
        /// Путь к месту сохранения экземпляров, производных от Person.
        /// </summary>
        internal const string PathSaveOfPersons = @"F:\Projects\UlyanovProduseStore\UlyanovProduseStore.BL\bin\Debug\DataUsers";
        //TODO: При допилке на базу данных - удалить нахрен этот костыль.

        internal string Name { get; set; }
        #endregion

        /// <summary>
        /// Возвращает Name экземпляра производного от Person..
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Name;
        }
    }
}

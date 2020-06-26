using System;

namespace UlyanovProduseStore.BL.Model
{
    /// <summary>
    /// Абстрактный класс для предотвращения дублирования кода. Содержит поле Name и переопределённый ToString.
    /// </summary>
    [Serializable]
    public abstract class Person
    {
        protected Person(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name), "Имя не может быть пустым!");

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

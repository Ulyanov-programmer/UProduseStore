using System;
using System.Reflection;
using System.Runtime.Serialization;

namespace UlyanovProduseStore.BL.Model
{
    /// <summary>
    /// Абстрактный класс для предотвращения дублирования кода. Содержит поле Name и переопределённый ToString.
    /// </summary>
    [DataContract]
    public abstract class Person
    {
        protected Person(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name), "Имя не может быть пустым!");

            Name = name;
        }

        #region Params
        [DataMember]
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

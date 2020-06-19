using System;

namespace UlyanovProduseStore.BL.Model
{
    [Serializable]
    public abstract class Person
    {
        protected Person(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name), "Имя не может быть пустым!");

            Name = name;
            NAME_TO_USERDATA = $"user{Name}.dat"; //TODO: Сделать разные папки сохранения для сотрудников и клиентов
        }

        #region Params

        protected string NAME_TO_USERDATA;
        internal string Name { get; set; }
        #endregion

        public override string ToString()
        {
            return Name;
        }

        public string GetPathToUserData()
        {
            return NAME_TO_USERDATA;
        }
    }
}

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
        /// <param name="name"> Имя наследника Person. </param>
        /// <param name="passwordOrSecondName"> Пароль или фамилия наследника Person. </param>
        protected Person(string name, string passwordOrSecondName)
        {
            if (string.IsNullOrWhiteSpace(name) is false &&
                string.IsNullOrWhiteSpace(passwordOrSecondName) is false)
            {
                PasswordOrSecondName = passwordOrSecondName;
                Name = name;
            }
        }

        #region Params

        public string Name { get; set; }
        public string PasswordOrSecondName { get; set; }

        #endregion

        #region overrides

        /// <summary>
        /// 
        /// </summary>
        /// <returns> Значение Name экземпляра, производного от Person, если оно не пусто. Иначе - null. </returns>
        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(Name) is false)
            {
                return Name;
            }
            return null;
        }

        #endregion
    }
}

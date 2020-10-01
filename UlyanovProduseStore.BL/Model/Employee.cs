namespace UlyanovProduseStore.BL.Model
{
    /// <summary>
    /// Класс сотрудника, содержащий информацию о нём, такую как имя, фамилию и т.п .
    /// </summary>
    public class Employee : Person
    {
        public Employee() : base("X", "X") { }

        /// <summary>
        /// Создаёт новый экземпляр Employee.
        /// </summary>
        /// <param name="name"> Имя нового экземпляра Employee. </param>
        /// <param name="secondName"> Фамилия нового экземпляра Employee. </param>
        public Employee(string name, string secondName) : base(name, secondName)
        {

        }

        #region params

        public int Id { get; set; }
        public decimal Salary { get; set; }

        #endregion
    }
}

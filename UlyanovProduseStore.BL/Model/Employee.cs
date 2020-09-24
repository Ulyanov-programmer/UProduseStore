using System;

namespace UlyanovProduseStore.BL.Model
{
    public class Employee : Person
    {
        public Employee() : base("X", "X")
        {
        }

        public Employee(string name, string secondName) : base(name, secondName)
        {

        }

        #region params

        public int Id { get; set; }
        public string SecondName { get; set; }
        public decimal Salary { get; set; }

        #endregion
    }
}

using System;

namespace UlyanovProduseStore.BL.Model
{
    [Serializable]
    public class Employee : Person
    {
        private int ID = 5; //TODO: Вообще, тут должна быть проверка на наличие такого сотрудника в БД.

        public Employee(string name) : base(name)
        {
        }

        public int GetID()
        {
            return ID;
        }
    }
}

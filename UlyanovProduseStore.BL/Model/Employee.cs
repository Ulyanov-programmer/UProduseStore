using System;
using System.Runtime.Serialization;

namespace UlyanovProduseStore.BL.Model
{
    [DataContract]
    public class Employee : Person //TODO: После допилки под БД, дополнить описание.
    {
        [DataMember]
        private int ID = 5; //TODO: Запилить регистрацию сотрудников. 

        /// <summary>
        /// Создаёт новый экземпляр класса Employee.
        /// </summary>
        /// <param name="name"> Его имя. </param>
        public Employee(string name) : base(name)
        {
        }

        /// <summary>
        /// Возвращает ID сотрудника. 
        /// </summary>
        /// <returns></returns>
        public int GetID()
        {
            return ID;
        }
    }
}

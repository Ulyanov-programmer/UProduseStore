using System;
using UlyanovProduseStore.BL;
using UlyanovProduseStore.BL.Controller;
using UlyanovProduseStore.BL.Model;

namespace UlyanovProduseStore.WarehouseTerminal
{
    class Program
    {
        static void Main(string[] args)
        {
            var context = new UProduseStoreContext(ClientController.ConnectToMainServer);

            Console.WriteLine("здравствуйте. \n Для регистрации нового сотрудника нажмите R.");
            Console.WriteLine("Для работы с базой данных продуктов нажмите D.");

            switch (Console.ReadKey().Key)
            {
                case ConsoleKey.D:
                    break;
                case ConsoleKey.R:
                    Console.Write("Имя и фамилия нового сотрудника (через пробел): ");
                    string[] firstAndSecondName = Console.ReadLine().Split(' ');
                    ClientController.RegistrationOfPerson<Employee>(firstAndSecondName[0], firstAndSecondName[1], context);
                    break;
                default:
                    break;
            }
        }
    }
}

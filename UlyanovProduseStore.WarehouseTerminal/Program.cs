using System;
using System.Linq;
using System.Threading;
using UlyanovProduseStore.BL.Controller;
using UlyanovProduseStore.BL.Model;

namespace UlyanovProduseStore.WarehouseTerminal
{
    internal class Program
    {
        private static void Main()
        {
            #region preliminaryData

            var context = new UPSContext(UPSContext.StringConnectToMainServer);
            Employee employee = null;

            #endregion

            #region authentication

            while (employee is null)
            {
                Console.WriteLine("Здравствуйте сотрудник, введите ваш ID.");

                if (int.TryParse(Console.ReadLine(), out int inputID) == false)
                {
                    continue;
                }

                employee = context.Employees.Find(inputID);
            }

            #endregion

            var productsFromDb = context.Products.ToList();

            #region actions

            while (true)
            {
                Console.Clear();
                Console.WriteLine($"Добро пожаловать, {employee.Name}!");


                Console.WriteLine("Вам доступно добавление продуктов, нажмите на A что-бы добавить их.");
                switch (Console.ReadKey(true).Key)
                {
                    #region key A (adding new product)

                    case ConsoleKey.A:

                        var newProduct = new Product();

                        Console.Write("Название: ");
                        newProduct.Name = (Console.ReadLine());

                        Console.Write("Стоимость (в рублях): ");
                        newProduct.Cost = Convert.ToDecimal(Console.ReadLine());

                        if (string.IsNullOrWhiteSpace(newProduct.Name) == false &&
                            newProduct.Cost > 0)
                        {
                            if (ProductController.AddProducts(context, newProduct, productsFromDb))
                            {
                                Console.WriteLine("Продукт был успешно добавлен.");
                            }
                            else
                            {
                                Console.WriteLine("Продукт не был добавлен!");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Проверьте корректность вводимых данных!");
                        }

                        Thread.Sleep(6000);
                        break;

                        #endregion
                }
            }

            #endregion
        }
    }
}

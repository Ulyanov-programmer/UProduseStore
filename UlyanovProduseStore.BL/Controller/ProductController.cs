using System;
using System.Collections.Generic;
using System.IO;
using UlyanovProduseStore.BL.Model;
using System.Linq;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;

namespace UlyanovProduseStore.BL.Controller
{
    public static class ProductController
    {
        public static List<Product> LoadProducts(string pathOfLoad)
        {
            using (var context = new UPSEmployeeContext(pathOfLoad))
            {
                try
                {
                    if (context.Products.Count() > 0)
                    {
                        var products = context.Products.ToList();
                        return products;
                    }
                    else
                    {
                        throw new Exception("Не удалось загрузить список продуктов. Был создан список по умолчанию.");
                        //TODO: Проверить работоспособность. 
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    var productsNull = new List<Product>();
                    return productsNull;
                }
            }
        }

        public static void AddProducts(string pathOfLoad)
        {
            List<Product> products = new List<Product>();

            using (var context = new UPSEmployeeContext(pathOfLoad))
            {
                products = context.Products.ToList();   

                while (true)
                {
                    Console.WriteLine(@"Ведите название и стоимость продукта.");

                    Console.Write("Название: ");
                    string inputName = Console.ReadLine();

                    if (products.Any(x => x.Name == inputName))
                    {
                        Console.WriteLine("Продукт с такими именем уже существует!");
                        Thread.Sleep(5000);
                        Console.Clear();
                        continue;
                    }

                    Console.Write("Стоимость (в рублях): ");
                    decimal.TryParse(Console.ReadLine(), out decimal cost);


                    products.Add(new Product(inputName, cost));

                    Console.WriteLine(@"Продукт добавлен, но изменения не сохранены.");
                    Console.WriteLine(@"Если более не собираетесь их добавлять, введите ""stop"". В ином случае - введите что угодно или нажмите Enter.");

                    if (Console.ReadLine() == "stop")
                    {
                        Console.Clear();
                        break;
                    }
                }

                context.SaveChanges();
                Console.WriteLine("Добавление продуктов завершено, изменения сохранены.");
                Thread.Sleep(5000);
            }
        }

        #region GettersSetters

        /// <summary>
        /// Возвращает поле Name продукта. 
        /// </summary>
        /// <param name="product">Экземпляр Product из которого будет идти считывание.</param>
        /// <returns></returns>
        public static string GetName(Product product)
        {
            return product.Name;
        }

        /// <summary>
        /// Возвращает поле Cost продукта.
        /// </summary>
        /// <param name="product">Экземпляр Product из которого будет идти считывание.</param>
        /// <returns></returns>
        public static decimal GetCost(Product product)
        {
            return product.Cost;
        }

        /// <summary>
        /// изменяет поле Name продукта. 
        /// </summary>
        /// <param name="newName">Новое имя экземпляра Product.</param>
        /// <param name="product">Экземпляр Product который будет изменён.</param>
        public static void SetName(string newName, Product product)
        {
            if (string.IsNullOrWhiteSpace(newName))
            {
                throw new ArgumentNullException(nameof(newName), "Название продукта не может быть пустым!");
            }
            product.Name = newName;
        }

        /// <summary>
        /// Изменяет поле Cost продукта.
        /// </summary>
        /// <param name="newCost">Новое значение Cost для экземпляра Product.</param>
        /// <param name="product">Экземпляр Product который будет изменён.</param>
        public static void SetCost(decimal newCost, Product product)
        {
            if (newCost <= 0)
            {
                throw new ArgumentNullException(nameof(newCost), "Стоимость продукта не может быть ниже или равна нулю!");
            }
            product.Cost = newCost;
        }
        #endregion
    }
}

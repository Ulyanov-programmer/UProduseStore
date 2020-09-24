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
        public static List<Product> LoadProducts(UPSContext context)
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
            catch (Exception)
            {
                var productsNull = new List<Product>();
                return productsNull;
            }
        }

        public static void AddProducts(UPSContext context)
        {
            var products = new List<Product>();
            products = context.Products.ToList();

            while (true) //TODO: Убрать элементы интерфейса, все данные должны идти из аргументов.
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

        #region GettersSetters

        /// <summary>
        /// Возвращает поле Name этого экземпляра Product. 
        /// </summary>
        /// <param name="product">Экземпляр Product из которого будет идти считывание.</param>
        /// <returns></returns>
        public static string GetName(Product product)
        {
            return product.Name;
        }

        /// <summary>
        /// Возвращает поле Cost этого экземпляра Product.
        /// </summary>
        /// <param name="product">Экземпляр Product из которого будет идти считывание.</param>
        /// <returns></returns>
        public static decimal GetCost(Product product)
        {
            return product.Cost;
        }

        /// <summary>
        /// изменяет поле Name этого экземпляра Product. 
        /// </summary>
        /// <param name="newName">Новое имя экземпляра Product.</param>
        /// <param name="product">Экземпляр Product который будет изменён.</param>
        public static string SetName(string newName, Product product)
        {
            if (string.IsNullOrWhiteSpace(newName))
            {
                return null;
            }
            product.Name = newName;
            return product.Name;
        }

        /// <summary>
        /// Изменяет поле Cost этого экземпляра Product.
        /// </summary>
        /// <param name="newCost">Новое значение Cost для экземпляра Product.</param>
        /// <param name="product">Экземпляр Product который будет изменён.</param>
        public static decimal SetCost(decimal newCost, Product product)
        {
            if (newCost <= 0)
            {
                return -1;
            }
            product.Cost = newCost;

            return product.Cost;
        }
        #endregion
    }
}

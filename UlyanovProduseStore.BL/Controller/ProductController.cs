using System;
using System.Collections.Generic;
using System.IO;
using UlyanovProduseStore.BL.Model;
using System.Linq;
using System.Threading;
using System.Runtime.Serialization.Json;

namespace UlyanovProduseStore.BL.Controller
{
    public static class ProductController
    {
        /// <summary>
        /// Возвращает список доступных продуктов из файла (путь - в Product.PathSaveOfProducts). 
        /// </summary>
        /// <returns> Возвращает заполненный лист с экземплярами Product, если файл не пуст (создаётся, если не найден).
        ///           Если он был пуст - создаёт базовое представление List Product, сериализует его и возвращает. </returns>
        public static List<Product> ShowProducts()
        {
            var jFormatter = new DataContractJsonSerializer(typeof(List<Product>));

            try
            {
                using (var stream = new FileStream(Product.PathSaveOfProducts, FileMode.OpenOrCreate))
                {
                    if (stream.Length > 0)
                    {
                        var products = jFormatter.ReadObject(stream) as List<Product>;
                        return products;
                    }
                    else
                    {
                        List<Product> products = new List<Product>();
                        jFormatter.WriteObject(stream, products);

                        return products;
                    }
                }
            }
            catch (Exception)
            {
                throw new FileLoadException("Не удалось загрузить данные о доступных продуктах.");
            }
        }

        /// <summary>
        /// На основе вводимых данных дополняет файл с Product-ами новым экземпляром.
        /// </summary>
        /// <param name="getEmployee">Объект-защита от несанкционированного доступа.</param>
        public static void AddProducts(Employee getEmployee) 
        {
            #region GetEmployee
            if (getEmployee == null)
            {
                throw new ArgumentNullException("Требуемый клиент пуст!");
            }
            #endregion
            List<Product> products = new List<Product>();
            var jFormatter = new DataContractJsonSerializer(typeof(List<Product>));

            if (File.Exists(Product.PathSaveOfProducts))
            {
                var stream = new FileStream(Product.PathSaveOfProducts, FileMode.Open);
                products = jFormatter.ReadObject(stream) as List<Product>;
                stream.Dispose();
                File.Delete(Product.PathSaveOfProducts); //Т.к файл некорректно перезаписывается, его нужно удалить. Это не так критично т.к это файл JSON.
            }
            using (var stream = new FileStream(Product.PathSaveOfProducts, FileMode.Create))
            {
                while (true)
                {
                    Console.WriteLine(@"Ведите название, стоимость и цифру категории продукта одной строкой, разделяя эти значения символом ""*"".");
                    Console.WriteLine("Доступные категории: ");
                    for (int item = 0; item < Product.Categories.Count; item++)
                    {
                        Console.WriteLine($"{item} = {Product.Categories[item]}");
                    }
                    Console.WriteLine("Обратите внимание, что пробелы будут удалены.");

                    string[] parms = Console.ReadLine().Replace(" ", "").Split('*');

                    decimal cost = decimal.Parse(parms[1]);
                    byte category = byte.Parse(parms[2]);

                    products.Add(new Product(parms.FirstOrDefault(), cost, category));


                    Console.WriteLine(@"Продукт добавлен, но изменения не сохранены.");
                    Console.WriteLine(@"Если более не собираетесь их добавлять, введите ""stop"". В ином случае - введите что угодно или нажмите Enter.");

                    if (Console.ReadLine() == "stop")
                    {
                        Console.Clear();
                        break;
                    }
                }
                jFormatter.WriteObject(stream, products);
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
        /// Возвращает поле Category 
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public static string GetCategory(Product product)
        {
            return product.Category;
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

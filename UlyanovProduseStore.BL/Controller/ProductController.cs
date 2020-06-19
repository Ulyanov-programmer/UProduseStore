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
        /// 
        /// </summary>
        /// <param name="employee"></param>
        /// <param name="getEmployee"></param>
        public static void AddProducts(Employee getEmployee) //TODO: Допилить описание.
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

        public static string GetName(Product product)
        {
            return product.Name;
        }
        public static decimal GetCost(Product product)
        {
            return product.Cost;
        }
        public static string GetCategory(Product product)
        {
            return product.Category;
        }
        public static void SetName(string newName, Product product)
        {
            if (string.IsNullOrWhiteSpace(newName))
            {
                throw new ArgumentNullException(nameof(newName), "Название продукта не может быть пустым!");
            }
            product.Name = newName;
        }
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

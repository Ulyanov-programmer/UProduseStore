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
        /// <summary>
        /// Возвращает список доступных продуктов из файла (путь - в Product.PathSaveOfProducts). 
        /// </summary>
        /// <returns> Возвращает заполненный лист с экземплярами Product, если файл не пуст (создаётся, если не найден).
        ///           Если он был пуст - создаёт базовое представление List Product, сериализует его и возвращает. </returns>
        public static List<Product> LoadProducts()
        {
            var bFormatter = new BinaryFormatter();
            using (var stream = new FileStream(Product.PathSaveOfProducts, FileMode.OpenOrCreate))
            {
                try
                {
                    if (stream.Length > 0)
                    {
                        var products = bFormatter.Deserialize(stream) as List<Product>;
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
                    var products = bFormatter.Deserialize(stream) as List<Product>;
                    return products;
                }

            }
        }

        /// <summary>
        /// На основе вводимых данных дополняет файл с Product-ами новым экземпляром.
        /// </summary>
        /// <param name="getEmployee">Объект-защита от несанкционированного доступа.</param>
        public static void AddProducts(Employee getEmployee) 
        {
            #region GetNullEmployee
            if (getEmployee == null)
            {
                throw new ArgumentNullException("Требуемый клиент пуст!");
            }
            #endregion
            List<Product> products = new List<Product>();
            var bFormatter = new BinaryFormatter();

            if (File.Exists(Product.PathSaveOfProducts))
            {
                using (var stream = new FileStream(Product.PathSaveOfProducts, FileMode.Open))
                {
                    products = bFormatter.Deserialize(stream) as List<Product>;
                }
            }
            else
            {
                Directory.CreateDirectory("Data");
            }
            using (var stream = new FileStream(Product.PathSaveOfProducts, FileMode.OpenOrCreate))
            {
                while (true)
                {
                    Console.WriteLine(@"Ведите название, стоимость и цифру категории продукта.");
                    Console.WriteLine("Доступные категории: ");
                    for (int item = 0; item < Product.Categories.Count; item++)
                    {
                        Console.WriteLine($"{item} = {Product.Categories[item]}");
                    }

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

                    Console.Write("Номер категории: ");
                    byte.TryParse(Console.ReadLine(), out byte category);


                    products.Add(new Product(inputName, cost, category));

                    Console.WriteLine(@"Продукт добавлен, но изменения не сохранены.");
                    Console.WriteLine(@"Если более не собираетесь их добавлять, введите ""stop"". В ином случае - введите что угодно или нажмите Enter.");

                    if (Console.ReadLine() == "stop")
                    {
                        Console.Clear();
                        break;
                    }
                }
                bFormatter.Serialize(stream, products);
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

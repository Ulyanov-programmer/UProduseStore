using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UlyanovProduseStore.BL.Model;
using System.Linq;

namespace UlyanovProduseStore.BL.Controller
{
    public class ProductController
    {
        #region Setters

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

        /// <summary>
        /// Выводит на экран консоли сериализованные экземпляры Product из файла.
        /// </summary>
        /// <returns> Возвращает лист с продуктами. Если файл не был обнаружен он его создаёт и сериализует туда пустой (не null) лист продуктов.
        /// </returns>
        public static List<Product> ShowProducts() //TODO: Вкорячить в TryCatch.
        {
            var binFormatter = new BinaryFormatter();

            using (var stream = new FileStream(@"products.soap", FileMode.OpenOrCreate))
            {
                if (stream.Length > 0)
                {
                    List<Product> products = binFormatter.Deserialize(stream) as List<Product>;

                    foreach (var product in products)
                    {
                        Console.WriteLine($"{product.Name}, стоимость: {product.Cost} рублей, категория {product.Category}.");
                    }
                    return products;
                }
                else
                {
                    List<Product> products = new List<Product>();
                    binFormatter.Serialize(stream, products);

                    return products;
                }
            }
        }

        /// <summary>
        /// Сериализует один или несколько экземпляров Product.
        /// </summary>
        public static void AddProducts()
        {
            List<Product> products;
            var binFormatter = new BinaryFormatter();

            using (var stream = new FileStream("products.dat", FileMode.OpenOrCreate))
            {
                products = binFormatter.Deserialize(stream) as List<Product>;


                while (true)
                {
                    Console.WriteLine(@"Ведите название, стоимость и цифру категории продукта, разделяя эти значения символом ""*"".");
                    Console.Write("Доступные категории: ");
                    foreach (var item in Product.Categories)
                    {
                        Console.WriteLine(item);
                    }

                    string[] parms = Console.ReadLine().Replace(" ", "").Split('*');

                    decimal cost = decimal.Parse(parms[1]);
                    byte category = byte.Parse(parms.LastOrDefault());

                    products.Add(new Product(parms.FirstOrDefault(), cost, category));


                    Console.WriteLine(@"Продукт добавлен! Если более не собираетесь их добавлять, введите ""STOP"".");

                    if (Console.ReadLine() == "STOP")
                    {
                        Console.Clear();
                        Console.WriteLine("Добавление продуктов завершено.");
                        break;
                    }
                }
                
                binFormatter.Serialize(stream, products);
                Console.WriteLine("Изменения сохранены.");
            }
        }
    }
}

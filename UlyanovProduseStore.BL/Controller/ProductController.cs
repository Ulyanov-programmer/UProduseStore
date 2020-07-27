using System;
using System.Collections.Generic;
using UlyanovProduseStore.BL.Model;
using System.Linq;
using System.Threading;

namespace UlyanovProduseStore.BL.Controller
{
    public static class ProductController
    {
        /// <summary>
        /// Возвращает список доступных продуктов из файла (путь - в Product.PathSaveOfProducts). 
        /// </summary>
        /// <returns> Возвращает заполненный лист с экземплярами Product, если файл не пуст (создаётся, если не найден).
        ///           Если он был пуст - создаёт базовое представление List Product, сериализует его и возвращает. </returns>
        public static List<Product> LoadProducts(string pathLoad)
        {
            using (var context = new UProduseStoreContext(pathLoad))
            {
                try
                {
                    List<Product> products = context.Products.ToList();
                    return products;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return new List<Product>();
                }
            }
        }

        /// <summary>
        /// На основе вводимых данных дополняет файл с Product-ами новым экземпляром.
        /// </summary>
        /// <param name="getEmployee">Объект-защита от несанкционированного доступа.</param>
        public static void AddProducts(Employee getEmployee, string pathLoad) //TODO: Переделать комментарии.
        {
            #region GetNullEmployee
            if (getEmployee == null)
            {
                Console.WriteLine("Требуемый клиент пуст!");
                return;
            }
            #endregion
            List<Product> loadedProducts = new List<Product>();

            using (var context = new UProduseStoreContext(pathLoad))
            {
                loadedProducts = context.Products.ToList();

                while (true)
                {
                    Console.WriteLine(@"Ведите название, стоимость и цифру категории продукта.");
                    Console.Write("Название: ");
                    string inputName = Console.ReadLine();

                    if (loadedProducts.Any(x => x.Name == inputName))
                    {
                        Console.WriteLine("Продукт с такими именем уже существует!");
                        Thread.Sleep(5000);
                        continue;
                    }

                    Console.Write("Стоимость (в рублях): ");
                    decimal.TryParse(Console.ReadLine(), out decimal cost);

                    Console.Write("Номер категории: ");
                    byte.TryParse(Console.ReadLine(), out byte category);


                    loadedProducts.Add(new Product(inputName, cost));

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
        public static int AddProducts(Product inputProduct, string pathLoad)
        {
            using (var context = new UProduseStoreContext(pathLoad))
            {
                context.Products.Add(inputProduct);
                context.SaveChanges();

                int idOfInputProduct = context.Products.First(prod => prod.Name == inputProduct.Name).Id;
                return idOfInputProduct;
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
        //public static string GetCategory(Product product)
        //{
        //    return product;
        //}

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

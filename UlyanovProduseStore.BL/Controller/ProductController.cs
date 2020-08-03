using System;
using System.Collections.Generic;
using UlyanovProduseStore.BL.Model;
using System.Linq;
using System.Threading;

namespace UlyanovProduseStore.BL.Controller
{
    /// <summary>
    /// Класс-контроллер, с помощью статических методов которого можно работать с моделью Product.
    /// </summary>
    public static class ProductController
    {
        /// <summary>
        /// Возвращает объект List Product из базы данных.
        /// </summary>
        /// <param name="context"> Объект контекста, на основе которого будет создано подключение к БД. </param>
        /// <returns> Если данные были успешно загружены - возвращает List Product на основе доступных в БД. 
        ///           Если во время загрузки произошла ошибка, возвращает новый List Product имеющий базовое представление. </returns>
        public static List<Product> LoadProducts(UProduseStoreContext context)
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

        /// <summary>
        /// На основе вводимых во время выполнения метода данных дополняет таблицу с Product-ами в БД новым экземпляром.
        /// </summary>
        /// <param name="getEmployee">Объект-защита от несанкционированного доступа.</param>
        /// <param name = "context"> Объект контекста, на основе которого будет создано подключение к БД. </param>
        public static void AddProducts(Employee getEmployee, UProduseStoreContext context)
        {
            #region GetArguments
            if (getEmployee == null || context == null)
            {
                Console.WriteLine("входные данные некорректны!");
                return;
            }
            #endregion
            List<Product> loadedProducts = new List<Product>();
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
            context.SaveChangesAsync();
            Console.WriteLine("Добавление продуктов завершено, изменения сохранены.");
            Thread.Sleep(5000);
        }

        /// <summary>
        /// На основе экземпляра Product дополняет таблицу с Product-ами в БД новым экземпляром.
        /// </summary>
        /// <param name="inputProduct"> Экземпляр Product, данными которого будет пополнена соответствующая таблица в БД. </param>
        /// <param name="context"> Объект контекста, на основе которого будет создано подключение к БД. </param>
        /// <returns> Возвращает ID добавленного в БД экземпляра Product. Если контекст или Produc пусты, возвращает -1. </returns>
        public static int AddProducts(Product inputProduct, UProduseStoreContext context)
        {
            if (context != null && inputProduct != null)
            {
                context.Products.Add(inputProduct);
                context.SaveChanges();

                int idOfInputProduct = context.Products.First(prod => prod.Name == inputProduct.Name).Id;
                return idOfInputProduct;
            }
            return -1;
        }
        
        //TODO: Добавить метод удаления продукта из таблицы в БД.
        #region GettersSetters

        /// <summary>
        /// Возвращает поле Name экземпляра Product. 
        /// </summary>
        /// <param name="product">Экземпляр Product из которого будет идти считывание.</param>
        /// <returns></returns>
        public static string GetName(Product product)
        {
            return product.Name;
        }

        /// <summary>
        /// Возвращает поле Cost экземпляра Product.
        /// </summary>
        /// <param name="product">Экземпляр Product из которого будет идти считывание.</param>
        /// <returns></returns>
        public static decimal GetCost(Product product)
        {
            return product.Cost;
        }

        /// <summary>
        /// Изменяет поле Name экземпляра Product и сохраняет изменения в БД. 
        /// </summary>
        /// <param name="newName">Новое имя экземпляра Product.</param>
        /// <param name="inputProduct">Экземпляр Product который будет изменён.</param>
        public static bool SetName(string newName, Product inputProduct, UProduseStoreContext context)
        {
            if (string.IsNullOrWhiteSpace(newName) == false && context != null)
            {
                var productFromDB = context.Products.FirstOrDefault(prod => prod.Name == inputProduct.Name);
                if (productFromDB != default)
                {
                    productFromDB.Name = newName;

                    context.SaveChanges();
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Изменяет поле Cost экземпляра Product и сохраняет изменения в БД.
        /// </summary>
        /// <param name="newCost">Новое значение Cost для экземпляра Product.</param>
        /// <param name="product">Экземпляр Product который будет изменён.</param>
        public static bool SetCost(decimal newCost, Product product, UProduseStoreContext context)
        {
            if (newCost > 0 && context != null)
            {
                var productFromDB = context.Products.FirstOrDefault(prod => prod.Cost == product.Cost);
                if (productFromDB != default)
                {
                    productFromDB.Cost = newCost;

                    context.SaveChanges();
                    return true;
                }
            }
            return false;
        }
        #endregion
    }
}

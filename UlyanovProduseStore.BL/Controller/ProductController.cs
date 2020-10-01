using System;
using System.Collections.Generic;
using System.Linq;
using UlyanovProduseStore.BL.Model;

namespace UlyanovProduseStore.BL.Controller
{
    /// <summary>
    /// Класс-контроллер, содержащий методы для работы для работы с экземплярами Product и ими же, но из БД.
    /// </summary>
    public static class ProductController
    {
        /// <summary>
        /// Возвращает все экземпляры Product из базы данных.
        /// </summary>
        /// <param name="context"> Экземпляр контекста, необходимый для сохранения изменений в базе данных. </param>
        /// <returns> Если входные аргументы корректны и операция удалась - возвращает лист Product, 
        ///           иначе - null.
        /// </returns>
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
                }

            }
            catch (Exception)
            {
                var productsNull = new List<Product>();
                return productsNull;
            }
        }

        /// <summary>
        /// Добавляет экземпляр Product в БД.
        /// </summary>
        /// <param name="context"> Экземпляр контекста, необходимый для сохранения изменений в базе данных. </param>
        /// <param name="newProduct"> Добавляемый экземпляр Product. </param>
        /// <param name="productsFromDb"> Данные о экземплярах Product из БД,
        ///                               необходимые для защиты от ошибок (добавление уже существующего экземпляра и т.п).
        /// </param>
        /// <returns> True - если входные аргументы корректны, операция удалась и этот продукт не содержался в БД, иначе - false. </returns>
        public static bool AddProducts(UPSContext context, Product newProduct, List<Product> productsFromDb)
        {
            if (productsFromDb.Find(prod => prod.Name == newProduct.Name) is null &&
                context != null &&
                productsFromDb != null)
            {
                if (string.IsNullOrWhiteSpace(newProduct.Name) is false &&
                    newProduct.Cost > 0)
                {
                    context.Products.Add(newProduct);
                    context.SaveChanges();
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Приватный метод, необходимый для сохранения (синхронизации) изменений экземпляра Product с ним же в базе данных.
        /// </summary>
        /// <param name="product"> Изменяемый экземпляр Product. </param>
        /// <param name="context"> Экземпляр контекста, необходимый для сохранения изменений в базе данных. </param>
        /// <returns> True - если входные аргументы корректны и операция удалась, иначе - false. </returns>
        private static bool SaveProduct(Product product, UPSContext context)
        {
            try
            {
                var prodFromDb = context.Products.Find(product.Id);

                prodFromDb.Name = product.Name;
                prodFromDb.Cost = product.Cost;

                context.SaveChanges();

                if (prodFromDb.Name == product.Name &&
                    prodFromDb.Cost == product.Cost)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

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
        /// Изменяет поле Name экземпляра Product. 
        /// </summary>
        /// <param name="newName">Новое имя экземпляра Product.</param>
        /// <param name="product">Экземпляр Product который будет изменён.</param>
        /// <returns> Текущее имя этого экземпляра Product, если операция была успешно совершена, иначе - null. </returns>
        public static string SetName(string newName, Product product, UPSContext context)
        {
            if (string.IsNullOrWhiteSpace(newName) is false &&
                context != null &&
                product != null)
            {
                product.Name = newName;

                if (SaveProduct(product, context))
                {
                    return product.Name;
                }
            }
            return null;
        }

        /// <summary>
        /// Изменяет поле Cost экземпляра Product.
        /// </summary>
        /// <param name="newCost"> Новое значение Cost для экземпляра Product. </param>
        /// <param name="product"> Экземпляр Product который будет изменён. </param>
        public static decimal SetCost(decimal newCost, Product product, UPSContext context)
        {
            if (newCost > 0 &&
                context != null &&
                product != null)
            {
                product.Cost = newCost;
                if (SaveProduct(product, context))
                {
                    return product.Cost;
                }
            }
            return -1;
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using UlyanovProduseStore.BL.Model;
using System.Linq;

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
                }

            }
            catch (Exception)
            {
                var productsNull = new List<Product>();
                return productsNull;
            }
        }

        public static bool AddProducts(UPSContext context, Product newProduct, List<Product> productsFromDb)
        {
            if (productsFromDb.Find(prod => prod.Name == newProduct.Name) is null &&
                context != null &&
                productsFromDb != null)
            {
                context.Products.Add(newProduct);
                context.SaveChanges();
                return true;
            }
            return false;
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

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using UlyanovProduseStore.BL.Controller;

namespace UlyanovProduseStore.BL.Model.Tests
{
    [TestClass()]
    public class CreateProductTests
    {
        [TestMethod()]
        public void ProductTest()
        {
            ////Arrange
            //Random rnd = new Random();
            //string nameOfProduct = Guid.NewGuid().ToString();
            //decimal cost = rnd.Next(1, int.MaxValue);
            //int category = rnd.Next(0, 2);

            ////Act
            //Product product = new Product(nameOfProduct, cost, category);
            //Product productNullName = new Product(null, cost, category);
            //Product productNullCost = new Product(nameOfProduct, 0, category);
            //Product productMinusCost = new Product(nameOfProduct, -4000, category);
            //Product productOutRangeCaterory = new Product(nameOfProduct, cost, int.MaxValue);
            //Product productMinusNumberCaterory = new Product(nameOfProduct, cost, int.MinValue);
            //Product productFullNull = new Product(null, 0, 0);

            ////Assert
            //Assert.IsNotNull(product);
            //Assert.AreEqual(nameOfProduct, product.ToString());
            //Assert.AreEqual(cost, ProductController.GetCost(product));
            //Assert.IsNotNull(ProductController.GetCategory(product));
            //Assert.IsNull(ProductController.GetName(productNullName));
            //Assert.AreEqual(default, ProductController.GetCost(productNullCost));
            //Assert.AreEqual(default, ProductController.GetCost(productMinusCost));
            //Assert.IsNull(ProductController.GetCategory(productOutRangeCaterory));
            //Assert.IsNull(ProductController.GetCategory(productMinusNumberCaterory));

            //Assert.IsNull(ProductController.GetCategory(productFullNull));
            //Assert.AreEqual(default, ProductController.GetCost(productFullNull));
            //Assert.IsNull(ProductController.GetName(productFullNull));
        }
    }
}